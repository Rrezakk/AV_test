using System.Net.Http;

namespace AV_test.Parsing.Queries
{
    namespace AV_test.Queries
    {
        
        public class GetWoodDealsQuery:IApiQuery
        {
            public HttpRequestMessage Message =>RequestMessage();
            private readonly int _count;
            private readonly int _page;
            public GetWoodDealsQuery(int count, int page)
            {
                _count = count;
                _page = page;
            }
            private HttpRequestMessage RequestMessage()
            {
                // Define the query
                var query = $@"{{
                ""query"": ""query SearchReportWoodDeal($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {{\n searchReportWoodDeal(filter: $filter, pageable: {{number: $number, size: $size}}, orders: $orders) {{\n content {{\n sellerName\n sellerInn\n buyerName\n buyerInn\n woodVolumeBuyer\n woodVolumeSeller\n dealDate\n dealNumber\n __typename\n }}\n __typename\n }}\n}}\n"",
                ""variables"": {{
                    ""size"": {_count},
                    ""number"": {_page},
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

