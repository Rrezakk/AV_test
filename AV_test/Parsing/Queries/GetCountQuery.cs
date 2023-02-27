using System.Net.Http;

namespace AV_test.Parsing.Queries;

public class GetCountQuery:IApiQuery
{
    public HttpRequestMessage Message =>GetCountQuery.RequestMessage();
    private static HttpRequestMessage RequestMessage()
    {
        const string url = "https://www.lesegais.ru/open-area/graphql";
        // Set the GraphQL query
        const string query = "{\"query\":\"query SearchReportWoodDealCount($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {\\n  searchReportWoodDeal(filter: $filter, pageable: {number: $number, size: $size}, orders: $orders) {\\n    total\\n    number\\n    size\\n    overallBuyerVolume\\n    overallSellerVolume\\n    __typename\\n  }\\n}\\n\",\"variables\":{\"size\":20,\"number\":0,\"filter\":null},\"operationName\":\"SearchReportWoodDealCount\"}";
        // Create the HTTP request message
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(query, System.Text.Encoding.UTF8, "application/json")
        };
        return request;
    }
}