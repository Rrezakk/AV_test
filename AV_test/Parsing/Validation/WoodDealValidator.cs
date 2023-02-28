using AV_test.Parsing.Deserialization;

namespace AV_test.Parsing.Validation;

public static class WoodDealValidator
{
    public static (bool,string) IsValid(ReportWoodDeal deal)
    {
        if (deal.ContainsNullField())
        {
            return (false,"Contains null field");
        }
        if (!deal.BuyerInn.InLenRange(10, 12))
        {
            return (false, $"BuyerInn range invalid");
        }
        else if (!deal.SellerInn.InLenRange(10, 12))
        {
            return (false, $"SellerInn range invalid");

        }
        else if (deal.DealNumber!.Length != 28)
        {
            return (false, $"DealNumber length invalid");

        }
        else if (!(deal.BuyerName!.Length < 255))
        {
            return (false, $"BuyerName length invalid");

        }
        else if (!(deal.SellerName!.Length < 255))
        {
            return (false, $"SellerName length invalid");

        }
        else if (!deal.DealDate.IsValidDateString())
        {
            return (false, $"DealDate is not valid");

        }
        else if (deal.object_hash!.Length != 44)
        {
            return (false, $"object_hash length is invalid");

        }
        return (true,"");
    }
}
