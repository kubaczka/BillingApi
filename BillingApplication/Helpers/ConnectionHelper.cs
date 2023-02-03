using System.Net.Http.Headers;

namespace BillingApplication.Helpers
{
    public static class ConnectionHelper
    {
        private static readonly Dictionary<int, string> PaymentMap = new Dictionary<int, string>()
        {
            {1, @"http://localhost/" }
        };
        private static Uri GetGatewayAddress(int key)
        {
            if (PaymentMap.ContainsKey(key))

                return new Uri(PaymentMap[key]);
            else
                return new Uri(string.Empty);
        }
        public static HttpClient GetHttpClient(IHttpClientFactory clientFactory, int key)
        {
            var client = clientFactory.CreateClient();
            client.BaseAddress = GetGatewayAddress(key);
            return client;
        }
        public static HttpRequestMessage GetPaymentMessageTemplate(HttpMethod method, HttpContent content)
        {
            var result = new HttpRequestMessage()
            {
                Method = method,
                Content = content
            };

            return result;
        }
    }
}
