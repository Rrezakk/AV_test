using AV_test.Parsing.Queries;
using System.Net.Http;

namespace AV_test.Parsing;

public class QueryExecutor
{
    private readonly HttpClient _client;
    public QueryExecutor()
    {
        _client = new HttpClient();
        // ReSharper disable once StringLiteralTypo
        _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36");
    }
    public string? Execute(IApiQuery query)
    {
        var response = _client.SendAsync(query.Message).Result;
        if (!response.IsSuccessStatusCode) return null;
        var result = response.Content.ReadAsStringAsync().Result;
        return result;
    }
}