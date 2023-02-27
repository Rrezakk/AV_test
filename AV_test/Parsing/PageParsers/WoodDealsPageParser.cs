using AV_test.DAL.Interfaces;
using AV_test.Parsing.Deserialization.WoodDeals;
using AV_test.Parsing.Queries;
using AV_test.Parsing.Queries.AV_test.Queries;
using Newtonsoft.Json;
using System;

namespace AV_test.Parsing.PageParsers;

public class WoodDealsPageParser
{
    private readonly IWoodDealsRepository _woodDealsRepository;
    private readonly QueryExecutor _queryExecutor;
    public WoodDealsPageParser(IWoodDealsRepository woodDealsRepository,QueryExecutor queryExecutor)
    {
        _woodDealsRepository = woodDealsRepository;//so dont have DI container...
        _queryExecutor = queryExecutor;//so dont have DI container...

    }
    public void DoCycle()
    {
        var dealsCountJson = _queryExecutor.Execute(new GetCountQuery());
        var response = JsonConvert.DeserializeObject<SearchReportWoodDealResponse>(dealsCountJson ?? string.Empty)?.data?.searchReportWoodDeal;
        const int entitiesPerRequest = 20;
        var totalEntities = response?.total ?? 0;
        var ctr = 0;
        var pagesTotal = totalEntities / entitiesPerRequest+1;
        for (var i = 7597; i < pagesTotal; i++)
        {
            Console.WriteLine($"Page: {i}");
            var q = new GetWoodDealsQuery(entitiesPerRequest,i);
            var resp = _queryExecutor.Execute(q);
            if (resp == null) continue;
            var deals = WoodDealDeserializer.GetDeals(resp);
            foreach (var deal in deals)
            {
                deal.object_hash = ObjectHashingHelper.ComputeSha256Hash(deal);
                ProcessDeal(deal);
            }
            ctr += deals.Count;
        }
        Console.WriteLine($"Processed total: {ctr} entities");
    }
    private void ProcessDeal(ReportWoodDeal deal)
    {
        
        //there is some place for validation
        // not valid - log and skip
        
        Console.Write($"Manipulating: {deal.object_hash}");
        //get object from db
        //check edit by hash
        //edit object or add to db
        var dbDeal = _woodDealsRepository.Get(deal);
        if (dbDeal == null)//creating
        {
            Console.WriteLine(" - creating entity");
            _woodDealsRepository.Create(deal);
            return;
        }
        if (dbDeal.object_hash == deal.object_hash) return;//editing 
        Console.WriteLine(" - editing entity");
        _woodDealsRepository.Edit(deal);
    }
}
