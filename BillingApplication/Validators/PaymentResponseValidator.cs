using BillingApplication.Abstractions;
using BillingApplication.Models;
using FluentValidation;

namespace BillingApplication.Validators
{
    public class PaymentResponseValidator : AbstractValidator<PaymentResponse>
    {
        public PaymentResponseValidator() {
            RuleFor(user => user.UserId).NotEmpty();
            RuleFor(order => order.OrderNumber).NotEmpty();
        }
    }
}
