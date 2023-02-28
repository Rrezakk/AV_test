using AV_test.Parsing.Deserialization;
using System;
using System.Text.RegularExpressions;
using static System.DateTime;

namespace AV_test.Parsing.Validation;

public static class ValidationExtensions
{
    private static readonly Regex  DateRegex = new Regex(@"^\d{4}-\d{2}-\d{2}$");
    private static readonly Regex InnRegex = new Regex(@"^[\d+]{10,12}$");
    private static readonly int CurrentYear = Now.Year;
    private const int MinYear = 2000;
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
    public static bool IsValidDateString(this string? date)//2116-04-28 //maybe we should add an "reality check, like min and max year"
    {
        if (date == null) return false;
        if (!DateRegex.IsMatch(date)) return false;//is valid date in format yyyy-mm-dd
        var year = ValidationExtensions.ExtractYearFromString(date);//extracting year if 1-9 starting char and 4 length -> else -1
        return year!=-1 && year<=CurrentYear && year>=MinYear;//checking year for validance
    }
    private static int ExtractYearFromString(string input)
    {
        var index = input.IndexOf("-", StringComparison.Ordinal);
        if (index == -1) return index;
        var yearString = input.Substring(0,index);
        if (int.TryParse(yearString, out var year))
        {
            return year;
        }
        return -1;
    }
    public static bool IsValidInn(this string? inn)
    {
        return inn != null && InnRegex.IsMatch(inn);
    }
}
