using System.Text.Json.Serialization;

namespace BillingApplication.Models
{
    public struct PaymentResponse
    {

        [JsonPropertyName("orderNumber")]
        public int OrderNumber { get; set; }
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }
        [JsonPropertyName("responseCode")]      
        public int ResponseCode { get; set; }
        [JsonPropertyName("responseMessage")]
        public string ResponseMessage { get; set; } 
    }
}
