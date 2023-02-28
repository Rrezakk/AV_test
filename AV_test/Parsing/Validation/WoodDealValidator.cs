using AV_test.Parsing.Deserialization;

namespace AV_test.Parsing.Validation;

public static class WoodDealValidator
{
    public static (bool,string) IsValid(ReportWoodDeal deal)// I don't really know case-specific but maybe Inn can be empty strings?
    {
        if (deal.ContainsNullField())
        {
            return (false,"Contains null field");
        }
        if (!deal.BuyerInn.IsValidInn())
        {
            return (false, $"BuyerInn is invalid: '{deal.BuyerInn}'");
        }
        if (!deal.SellerInn.IsValidInn())
        {
            return (false, $"SellerInn is invalid: '{deal.SellerInn}'");

        }
        if (deal.DealNumber!.Length != 28)
        {
            return (false, "DealNumber length invalid");

        }
        if (!(deal.BuyerName!.Length < 255))
        {
            return (false, "BuyerName length invalid");

        }
        if (!(deal.SellerName!.Length < 255))
        {
            return (false, "SellerName length invalid");

        }
        if (!deal.DealDate.IsValidDateString())
        {
            return (false, $"DealDate is not valid: '{deal.DealDate}'");

        }
        if (deal.object_hash!.Length != 44)
        {
            return (false, "object_hash length is invalid");

        }
        return (true,"");
    }
}
