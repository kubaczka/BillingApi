namespace BillingApplication.Abstractions
{
    public interface IPaymentGateway
    {
        public Guid Id { get; set; }    
        public string? Endpoint { get; set; }
    }
}
