using BillingApplication.Abstractions;
using BillingApplication.Helpers;
using BillingApplication.Models;
using BillingApplication.Validators;
using FluentValidation;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;

namespace BillingApplication.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly ILogger _logger;
        private readonly IHttpClientFactory _clientFactory;
        public PaymentService(ILogger<PaymentService> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }
        public async Task<PaymentResponse> ProcessPayment(int gatewayId, Guid userId, decimal amount, CancellationToken token = default)
        {
            _logger.LogInformation($"Payment service is starting connection to gateway id: {gatewayId}");
            var parsedResponse = new PaymentResponse();
            var client = ConnectionHelper.GetHttpClient(_clientFactory, gatewayId);
            var content = new StringContent(string.Empty, MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Json));
            var message = ConnectionHelper.GetPaymentMessageTemplate(HttpMethod.Get, content);
            var clientValidator = new HttpClientValidator();

            await clientValidator.ValidateAndThrowAsync<HttpClient>(client);

            var response = await client.SendAsync(message, token);
            response.EnsureSuccessStatusCode();
           
            var responseString = await response.Content.ReadAsStringAsync(token);
            parsedResponse = JsonSerializer.Deserialize<PaymentResponse>(responseString);

            var paymentValidator = new PaymentResponseValidator();
            await paymentValidator.ValidateAndThrowAsync<PaymentResponse>(parsedResponse, token);

            return await Task.FromResult<PaymentResponse>(parsedResponse);
        }
    }
}
