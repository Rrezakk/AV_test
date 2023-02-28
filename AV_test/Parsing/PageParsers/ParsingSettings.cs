namespace AV_test.Parsing.PageParsers;

public struct ParsingSettings
{
    public ParsingSettings()
    {
    }
    public int SampleSize { get; set; } = 20;
    public int DelayBetweenRequests { get; set; } = 200;//ms
    
}
