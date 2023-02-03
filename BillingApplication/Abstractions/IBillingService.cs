using BillingApplication.Models;

namespace BillingApplication.Abstractions
{
    public interface IBillingService
    {
        Task<BillingResponse> ProcessOrderAsync(IBillingRequest request, CancellationToken token = default);
    }
}
