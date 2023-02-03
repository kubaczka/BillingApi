using BillingApplication.Models;
using System.Text;

namespace BillingApplication.Helpers
{
    public static class ReceiptHelper
    {
        public static string GenerateRecepit(PaymentResponse paymentResponse)
        {
            var stringBuilder = new StringBuilder();
            if(CanGenerateReceipt(paymentResponse))
            {
                stringBuilder.AppendLine($"Order Number: {paymentResponse.OrderNumber}");
                stringBuilder.AppendLine($"User: {paymentResponse.UserId}");
                stringBuilder.AppendLine($"Response Code: {paymentResponse.ResponseCode}");
                stringBuilder.AppendLine($"Additional information: {paymentResponse.ResponseMessage}");
            }
            return stringBuilder.ToString();
        }
        private static bool CanGenerateReceipt(PaymentResponse paymentResponse)
        {
            return paymentResponse.OrderNumber != 0
                && paymentResponse.UserId != Guid.Empty
                && paymentResponse.ResponseCode == StatusCodes.Status200OK;
        }
    }
}
