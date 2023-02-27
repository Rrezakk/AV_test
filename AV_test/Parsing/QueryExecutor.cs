using AV_test.Parsing.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace AV_test.Parsing
{
    public class QueryExecutor
    {
        private readonly HttpClient _client;
        public QueryExecutor()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36");
        }
        public string? Execute(IApiQuery query)
        {
            var response = _client.SendAsync(query.Message).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }
    }
}
