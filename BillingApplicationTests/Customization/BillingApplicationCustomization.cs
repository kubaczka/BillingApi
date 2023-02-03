using AutoFixture;
using AutoFixture.AutoNSubstitute;
using BillingApplication.Abstractions;
using BillingApplication.Models;
using MathNet.Numerics.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplicationTests.Customization
{
    /// <summary>
    /// Customizes values returned by test fixtures
    /// </summary>
    internal class BillingApplicationCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var guid = Guid.NewGuid();
          
            fixture.Register<IBillingRequest>(() =>
            {
                return new BillingRequest() {
                    Amount= new Random().NextDecimal(),
                    OptionalDescription= string.Empty,
                    OrderNumber = new Random().Next(1, 100),
                    PaymentGatewayId= new Random().Next(1, 100),
                    UserId = guid
                };
            });
          
            fixture.Customize<PaymentResponse>(composer =>
            composer
            .With(p => p.UserId, guid)
            .With(p => p.OrderNumber, new Random().Next(1, 100))
            .With(p => p.ResponseCode, 200)
            .With(p => p.ResponseMessage, string.Empty));

        }
    }
}
