using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AV_test.Parsing.Deserialization.WoodDeals
{
    public class ReportWoodDeal
    {
        [JsonProperty("sellerName")] public string SellerName { get; set; }

        [JsonProperty("sellerInn")] public string SellerInn { get; set; }

        [JsonProperty("buyerName")] public string BuyerName { get; set; }

        [JsonProperty("buyerInn")] public string BuyerInn { get; set; }

        [JsonProperty("woodVolumeBuyer")] public float WoodVolumeBuyer { get; set; }

        [JsonProperty("woodVolumeSeller")] public float WoodVolumeSeller { get; set; }

        [JsonProperty("dealDate")] public DateTime DealDate { get; set; }//in .net 6 DateOnly

        [JsonProperty("dealNumber")] public string DealNumber { get; set; }

        //[JsonProperty("__typename")] public string TypeName { get; set; }
    }

    public class PageReportWoodDeal
    {
        [JsonProperty("content")] public List<ReportWoodDeal> Content { get; set; }

        //[JsonProperty("__typename")] public string TypeName { get; set; }
    }

    public class Data
    {
        [JsonProperty("searchReportWoodDeal")] public PageReportWoodDeal SearchReportWoodDeal { get; set; }
    }

    public class RootObject
    {
        [JsonProperty("data")] public Data Data { get; set; }
    }
}
