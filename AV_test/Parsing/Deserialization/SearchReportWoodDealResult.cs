// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global
namespace AV_test.Parsing.Deserialization;

public class SearchReportWoodDealResponse
{
    public SearchReportWoodDealData? data { get; set; }
}
public class SearchReportWoodDealData
{
    public SearchReportWoodDealResult? searchReportWoodDeal { get; set; }
}
public class SearchReportWoodDealResult
{
    public int total { get; set; }
    //public int number { get; set; }
    //public int size { get; set; }
    //public double overallBuyerVolume { get; set; }
    //public double overallSellerVolume { get; set; }
}//some props not used yet

