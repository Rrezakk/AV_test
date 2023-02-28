using Newtonsoft.Json;
using System.Collections.Generic;

namespace AV_test.Parsing.Deserialization;

public static class WoodDealDeserializer
{
    public static List<ReportWoodDeal> GetDeals(string json)
    {
        var root = JsonConvert.DeserializeObject<RootObject>(json);
        return root == null ? new List<ReportWoodDeal>():root.Data?.SearchReportWoodDeal?.Content ?? new List<ReportWoodDeal>();
    }
}