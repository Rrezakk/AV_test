using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AV_test.Parsing.Deserialization.WoodDeals
{
    public class ReportWoodDeal
    {
        public string SellerName { get; set; }
        public string SellerInn { get; set; }
        public string BuyerName { get; set; }
        public string BuyerInn { get; set; }
        public float WoodVolumeBuyer { get; set; }
        public float WoodVolumeSeller { get; set; }
        public DateTime DealDate { get; set; }//in .net 6 DateOnly
        public string DealNumber { get; set; }
        [JsonIgnore]
        public string object_hash { get; set; }
    }
    public class PageReportWoodDeal
    {
        public List<ReportWoodDeal> Content { get; set; }
    }
    public class Data
    {
        public PageReportWoodDeal SearchReportWoodDeal { get; set; }
    }
    public class RootObject
    {
        public Data Data { get; set; }
    }
}
