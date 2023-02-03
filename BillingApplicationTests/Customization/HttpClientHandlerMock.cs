using BillingApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BillingApplicationTests.Customization
{
    public class HttpClientHandlerMock<T> : HttpClientHandler
    {
        private readonly HttpStatusCode statusCode;
        private readonly T body;
        private readonly PaymentResponse responseMock;

        public HttpClientHandlerMock(HttpStatusCode statusCode, PaymentResponse responseMock)
        {
            this.statusCode = statusCode;
            this.responseMock= responseMock; 
        }

        public HttpClientHandlerMock(HttpStatusCode statusCode, T body)
        {
            this.body = body;
            this.statusCode = statusCode;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage { StatusCode = this.statusCode };

            if (this.body != null)
            {
                var serializedResponse = JsonSerializer.Serialize(this.responseMock);
                response.Content = new StringContent(serializedResponse);
            }

            return await Task.FromResult(response);
        }
    }
}
