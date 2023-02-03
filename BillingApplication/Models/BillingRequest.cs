using BillingApplication.Abstractions;

namespace BillingApplication.Models
{
    public class BillingRequest : IBillingRequest
    {
        public int OrderNumber { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public int PaymentGatewayId { get; set; }
        public string? OptionalDescription { get; set; }
    }
}
