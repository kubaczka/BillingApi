using AutoFixture;
using AutoFixture.AutoNSubstitute;
using BillingApplication.Abstractions;
using BillingApplication.Models;
using BillingApplication.Services;
using BillingApplicationTests.Customization;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit.Sdk;
using FluentAssertions;
using BillingApplication.Helpers;
using NSubstitute.ExceptionExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BillingApplicationTests.Tests
{
    [TestClass]
    public class BillingServiceTests
    {
        private readonly IFixture _fixture;
        private readonly ILogger<BillingService> _logger;
        public BillingServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoNSubstituteCustomization());
            _fixture.Customize(new BillingApplicationCustomization());
            _logger = _fixture.Create<ILogger<BillingService>>();
        }
        [TestMethod]
        public async Task When_ValidRequest_Then_ReturnReceipt()
        {
            //a           
            var fakeRequest = _fixture.Create<BillingRequest>();
            var expectedResult = new PaymentResponse
            {
                OrderNumber = fakeRequest.OrderNumber,
                UserId = fakeRequest.UserId,
                ResponseMessage = string.Empty,
                ResponseCode = StatusCodes.Status200OK
            };
            var fakePaymentService = _fixture.Create<IPaymentService>();
            fakePaymentService.ProcessPayment(fakeRequest.PaymentGatewayId, fakeRequest.UserId, fakeRequest.Amount)
                .Returns(expectedResult);
            var billingService = new BillingService(_logger, fakePaymentService);
            
            //aa
            var result = await billingService.ProcessOrderAsync(fakeRequest);
            
            //aaa
            result.Receipt.Should().NotBeEmpty();

        }
        [TestMethod]
        public async Task When_NotValidRequest_Then_ThrowValidationException()
        {
            //a
            var fakeService = _fixture.Create<IBillingService>();
            var fakeRequest = new BillingRequest();
            var expectedResult = new PaymentResponse { };
                      
            var fakePaymentService = _fixture.Create<IPaymentService>();
            fakePaymentService.ProcessPayment(0, Guid.Empty, 0)
                .Returns(expectedResult);
            var billingService = new BillingService(_logger, fakePaymentService);
            
            //aa
            Func<Task> act = async () => await billingService.ProcessOrderAsync(fakeRequest);

            //aaa
            await act.Should().ThrowAsync<ValidationException>();
        }
        [TestMethod]
        public async Task When_NotValidResponse_Then_ReturnNoReceipt()
        {
            //a
            var fakeService = _fixture.Create<IBillingService>();
            var fakeRequest = _fixture.Create<BillingRequest>();
            var expectedResult = new PaymentResponse
            {

            };
            var fakeReceipt = ReceiptHelper.GenerateRecepit(expectedResult);
            var fakePaymentService = _fixture.Create<IPaymentService>();
            fakePaymentService.ProcessPayment(fakeRequest.PaymentGatewayId, fakeRequest.UserId, fakeRequest.Amount)
                .Returns(expectedResult);
            var billingService = new BillingService(_logger, fakePaymentService);

            //aa
            var result = await billingService.ProcessOrderAsync(fakeRequest);

            //aaa
            result.Receipt.Should().BeEmpty();

        }
    }
}