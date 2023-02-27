using System.Net.Http;

namespace AV_test.Parsing.Queries;

public interface IApiQuery
{
    HttpRequestMessage Message { get; }
}