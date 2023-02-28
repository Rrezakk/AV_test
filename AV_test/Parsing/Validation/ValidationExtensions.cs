using AV_test.Parsing.Deserialization;
using System;

namespace AV_test.Parsing.Validation;

public static class ValidationExtensions
{
    public static bool InLenRange(this string? str, int min, int max)
    {
        if (str == null) return false;
        var value = str.Length;
        return value >= min && value <= max;
    }
    public static bool ContainsNullField(this ReportWoodDeal deal)
    {
        return deal.BuyerName == null || deal.BuyerInn == null || deal.DealDate == null || deal.DealNumber == null ||
               deal.SellerInn == null || deal.SellerName == null || deal.object_hash==null;
    }
    public static bool IsValidDateString(this string? date)
    {
        throw new NotImplementedException();
    }
}
