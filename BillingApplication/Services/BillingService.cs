using BillingApplication.Abstractions;
using BillingApplication.Helpers;
using BillingApplication.Models;
using BillingApplication.Validators;
using FluentValidation;

namespace BillingApplication.Services
{
    public class BillingService : IBillingService
    {
       
        private readonly ILogger _logger;
      
        private readonly IPaymentService _paymentService;
        public BillingService(ILogger<BillingService> logger,  IPaymentService paymentService )
        {
            _logger = logger;           
            _paymentService = paymentService;
        }
        public async Task<BillingResponse> ProcessOrderAsync(IBillingRequest request, CancellationToken token = default)
        {
            _logger.LogInformation($"Billing service is starting to process order number {request.OrderNumber}");
            var validator = new BillingRequestValidator();

            await validator.ValidateAndThrowAsync<IBillingRequest>(request);

            var paymentResponse = await _paymentService.ProcessPayment(request.PaymentGatewayId, request.UserId, request.Amount, token);
            var billingResponse = new BillingResponse()
            {
                Receipt = ReceiptHelper.GenerateRecepit(paymentResponse),
                OrderNumber = paymentResponse.OrderNumber,
                Amount = request.Amount,
                PaymentGatewayId = request.PaymentGatewayId,
                UserId = paymentResponse.UserId
            };

            return await Task.FromResult<BillingResponse>(billingResponse);
        }


    }
}
