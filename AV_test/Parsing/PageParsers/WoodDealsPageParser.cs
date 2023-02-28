using AV_test.DAL.Interfaces;
using AV_test.Parsing.Deserialization;
using AV_test.Parsing.Queries;
using AV_test.Parsing.Queries.AV_test.Queries;
using Newtonsoft.Json;
using System;
using System.Threading;
using AV_test.Parsing.Validation;


namespace AV_test.Parsing.PageParsers;

public class WoodDealsPageParser
{
    private readonly IWoodDealsRepository _woodDealsRepository; // repository with exception - catching
    private readonly QueryExecutor _queryExecutor;              //wrap on httpClient
    private readonly ParsingSettings _settings;                 //struct to store settings
    public WoodDealsPageParser(ParsingSettings settings,IWoodDealsRepository woodDealsRepository,QueryExecutor queryExecutor)
    {
        _woodDealsRepository = woodDealsRepository;//so dont have DI container...
        _queryExecutor = queryExecutor;//so dont have DI container...
        _settings = settings;

    }
    public void DoCycle()
    {
        var dealsCountJson = _queryExecutor.Execute(new GetCountQuery());//getting all deals count
        var response = JsonConvert.DeserializeObject<SearchReportWoodDealResponse>(dealsCountJson ?? string.Empty)?.data?.searchReportWoodDeal;//deserializing
        var totalEntities = response?.total ?? 0;
        var ctr = 0;// simple metrics 
        var pagesTotal = totalEntities / _settings.SampleSize+1;//because API calls based on pages
        for (var i = 0; i < pagesTotal; i++)
        {
            Console.WriteLine($"Page: {i}");
            var q = new GetWoodDealsQuery(_settings.SampleSize,i);   //creating querry
            var resp = _queryExecutor.Execute(q);                        //executing it
            if (resp == null) continue;
            var deals = WoodDealDeserializer.GetDeals(resp);//deserialization
            foreach (var deal in deals)
            {
                deal.object_hash = ObjectHashingHelper.ComputeSha256Hash(deal);//computing hash for easier change-tracking
                ProcessDeal(deal);                                             //processing deal by deal
            }
            ctr += deals.Count;
            Thread.Sleep(_settings.DelayBetweenRequests);                      //requests delay - stopwatch can be added for counting processing time
                                                                               //then delay = DelayBetweenRequests-processingTime
        }
        Console.WriteLine($"Processed total: {ctr} entities");
    }
    private void ProcessDeal(ReportWoodDeal deal)
    {
        var (validationResult, errorMessage) = WoodDealValidator.IsValid(deal);//validation of all props
        if (validationResult == false)
        {
            Console.WriteLine($"Not valid deal with hash {deal.object_hash} -> {errorMessage}");
            return;
        }

        //if deal is valid -> operating with db
        
        Console.Write($"Manipulating: {deal.object_hash}");
        var dbDeal = _woodDealsRepository.Get(deal);//get entity from db
        if (dbDeal == null)//creating
        {
            Console.WriteLine(" - creating entity");
            _woodDealsRepository.Create(deal);//saving entity to db
            return;
        }
        if (dbDeal.object_hash == deal.object_hash) return;//editing entity in db
        Console.WriteLine(" - editing entity");
        _woodDealsRepository.Edit(deal);
    }
}
