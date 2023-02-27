namespace AV_test.Parsing.Deserialization.WoodDeals;

public class SearchReportWoodDealResponse
{
    public SearchReportWoodDealData data { get; set; }
}

public class SearchReportWoodDealData
{
    public SearchReportWoodDealResult searchReportWoodDeal { get; set; }
}

public class SearchReportWoodDealResult
{
    public int total { get; set; }
    public int number { get; set; }
    public int size { get; set; }
    public double overallBuyerVolume { get; set; }
    public double overallSellerVolume { get; set; }
    //public string __typename { get; set; }
}
