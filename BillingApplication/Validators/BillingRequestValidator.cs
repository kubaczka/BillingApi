using BillingApplication.Abstractions;
using BillingApplication.Models;
using FluentValidation;

namespace BillingApplication.Validators
{
    public class BillingRequestValidator : AbstractValidator<IBillingRequest>
    {
        public BillingRequestValidator() {
            RuleFor(order => order.OrderNumber).NotEmpty();
            RuleFor(payment=> payment.PaymentGatewayId).NotEmpty();
            RuleFor(user => user.UserId).NotEmpty();
            RuleFor(amount => amount.Amount).NotEmpty();    
        }
    }
}
