using System.Net.Http;

namespace AV_test.Parsing.Interfaces
{
    public interface IApiQuery
    {
        HttpRequestMessage Message { get; }
    }
}
