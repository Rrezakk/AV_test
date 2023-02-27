using AV_test.Parsing.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace AV_test.Parsing.Queries
{
    namespace AV_test.Queries
    {
        
        public class GetWoodDealsQuery:IApiQuery
        {
            public HttpRequestMessage Message =>RequestMessage();
            private readonly int _count;
            private readonly int _position;
            public GetWoodDealsQuery(int count, int position)
            {
                this._count = count;
                this._position = position;
            }
            private HttpRequestMessage RequestMessage()
            {
                // Define the query
                var query = $@"{{
                ""query"": ""query SearchReportWoodDeal($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {{\n searchReportWoodDeal(filter: $filter, pageable: {{number: $number, size: $size}}, orders: $orders) {{\n content {{\n sellerName\n sellerInn\n buyerName\n buyerInn\n woodVolumeBuyer\n woodVolumeSeller\n dealDate\n dealNumber\n __typename\n }}\n __typename\n }}\n}}\n"",
                ""variables"": {{
                    ""size"": {_count},
                    ""number"": {_position},
                    ""filter"": null,
                    ""orders"": null
                }},
                ""operationName"": ""SearchReportWoodDeal""
            }}";
                // Create a new HttpRequestMessage with the query as the content
                var request = new HttpRequestMessage(HttpMethod.Post, "https://www.lesegais.ru/open-area/graphql")
                {
                    Content = new StringContent(query, System.Text.Encoding.UTF8, "application/json")
                };
                return request;
            }
        }
    }
}

