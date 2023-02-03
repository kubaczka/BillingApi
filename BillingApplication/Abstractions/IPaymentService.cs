using BillingApplication.Models;

namespace BillingApplication.Abstractions
{
    public interface IPaymentService
    {
        Task<PaymentResponse> ProcessPayment(int gatewayId, Guid userId, decimal amount, CancellationToken token = default);
    }
}
