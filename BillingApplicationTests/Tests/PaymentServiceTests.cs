using AutoFixture.AutoNSubstitute;
using AutoFixture;
using BillingApplication.Services;
using BillingApplicationTests.Customization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApplication.Abstractions;
using BillingApplication.Helpers;
using BillingApplication.Models;
using NSubstitute;
using System.Net;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BillingApplicationTests.Tests
{
    [TestClass]
    public class PaymentServiceTests
    {
        private readonly IFixture _fixture;
        private readonly ILogger<PaymentService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        public PaymentServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoNSubstituteCustomization());
            _fixture.Customize(new BillingApplicationCustomization());
            _logger = _fixture.Create<ILogger<PaymentService>>();
            _clientFactory = _fixture.Create<IHttpClientFactory>();
        }
        [TestMethod]
        public async Task When_ValidRequest_Then_ReturnResponse()
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
            var token = new CancellationToken(false);
            var clientHandler = new HttpClientHandlerMock<PaymentResponse>(HttpStatusCode.OK, expectedResult);
            var fakeClient = new HttpClient(clientHandler);
            _clientFactory.CreateClient().Returns<HttpClient>(fakeClient);
            var paymentService = new PaymentService(_logger, _clientFactory);
           
            //aa
            var result = await paymentService.ProcessPayment(1, fakeRequest.UserId, fakeRequest.Amount, token);
            
            //aaa
            result.Should().BeOfType<PaymentResponse>()
                .And.NotBeNull();

        }
        [TestMethod]
        public async Task When_ValidRequest_Then_ThrowValidationException()
        {
            //a          
            var fakeRequest = _fixture.Create<BillingRequest>();           
            var expectedResult = new PaymentResponse { };
            var token = new CancellationToken(false);
            var clientHandler = new HttpClientHandlerMock<PaymentResponse>(HttpStatusCode.OK, expectedResult);
            var fakeClient = new HttpClient(clientHandler);
            _clientFactory.CreateClient().Returns<HttpClient>(fakeClient);
            var paymentService = new PaymentService(_logger, _clientFactory);
            
            //aa           
            Func<Task> act = async () => await paymentService.ProcessPayment(1, fakeRequest.UserId, fakeRequest.Amount, token);
            
            //aaa
            await act.Should().ThrowAsync<ValidationException>();
        }
        [TestMethod]
        public async Task When_ClientConfigMissing_Then_ThrowValidationException()
        {
            //a           
            var fakeRequest = _fixture.Create<BillingRequest>();
            var expectedResult = new PaymentResponse { ResponseCode = StatusCodes.Status400BadRequest};
            var token = new CancellationToken(false);
            var clientHandler = new HttpClientHandlerMock<PaymentResponse>(HttpStatusCode.OK, expectedResult);
            var fakeClient = new HttpClient(clientHandler);
            fakeClient.BaseAddress = null;
            _clientFactory.CreateClient().Returns<HttpClient>(fakeClient);
            var paymentService = new PaymentService(_logger, _clientFactory);

            //aa           
            Func<Task> act = async () => await paymentService.ProcessPayment(1, fakeRequest.UserId, fakeRequest.Amount, token);

            //aaa
            await act.Should().ThrowAsync<ValidationException>();
        }
        [TestMethod]
        public async Task When_ValidRequest_Then_ReturnNOKResponse()
        {
            //a         
            var fakeRequest = _fixture.Create<BillingRequest>();
            var fakeHttpRequest = _fixture.Create<HttpRequestMessage>();
            var fakeHttpResponse = _fixture.Create<HttpResponseMessage>();
            var expectedResult = new PaymentResponse
            {
                OrderNumber = fakeRequest.OrderNumber,
                UserId = fakeRequest.UserId,
                ResponseMessage = string.Empty,
                ResponseCode = StatusCodes.Status200OK
            };
            var token = new CancellationToken(false);
            var clientHandler = new HttpClientHandlerMock<PaymentResponse>(HttpStatusCode.InternalServerError, expectedResult);
            var fakeClient = new HttpClient(clientHandler);
            _clientFactory.CreateClient().Returns<HttpClient>(fakeClient);
            var paymentService = new PaymentService(_logger, _clientFactory);
            
            //aa
            Func<Task> act = async () => await paymentService.ProcessPayment(1, fakeRequest.UserId, fakeRequest.Amount, token);
           
            //aaa
            await act.Should().ThrowAsync<HttpRequestException>();

        }
    }
}
