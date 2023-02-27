using AV_test.DAL.Interfaces;
using AV_test.Parsing.Deserialization.WoodDeals;
using AV_test.Parsing.Queries;
using AV_test.Parsing.Queries.AV_test.Queries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AV_test.Parsing.PageParsers;

public class WoodDealsPageParser
{
    private readonly IWoodDealsRepository _woodDealsRepository;
    private readonly QueryExecutor _queryExecutor;
    public WoodDealsPageParser(IWoodDealsRepository woodDealsRepository)
    {
        _woodDealsRepository = woodDealsRepository;
        _queryExecutor = new QueryExecutor();

    }
    public void DoCycle()
    {
        var dealsQuery = new GetCountQuery();
        var dealsCountJson = _queryExecutor.Execute(dealsQuery);
        var response = JsonConvert.DeserializeObject<SearchReportWoodDealResponse>(dealsCountJson ?? string.Empty)?.data.searchReportWoodDeal;
        var index = 1;//1
        var entitiesPerRequest = 50;
        var totalEntities = response?.total ?? 0;
        var ctr = 0;
        while (index+entitiesPerRequest-1 < totalEntities)
        {
            Console.WriteLine($"Iteration: {index} {totalEntities - (index+entitiesPerRequest-1)}");
            var q = new GetWoodDealsQuery(entitiesPerRequest,index);
            var resp = _queryExecutor.Execute(q);
            var deals = WoodDealDeserializer.GetDeals(resp);
            DataManipulation(deals);
            ctr += deals.Count;
            index += entitiesPerRequest;
        }
        if (index+entitiesPerRequest-1!=totalEntities)
        {
            var q = new GetWoodDealsQuery(totalEntities - (index+entitiesPerRequest-1),index);
            var resp = _queryExecutor.Execute(q);
            var deals = WoodDealDeserializer.GetDeals(resp);
            ctr += deals.Count;
            DataManipulation(deals);
        }
        Console.WriteLine($"Processed: {ctr} entities");
    }
    private async void DataManipulation(List<ReportWoodDeal> deals)
    {
        
    }
}
