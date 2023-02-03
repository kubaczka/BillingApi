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
using BillingApplication.Controllers;
using BillingApplication.Models;
using System.Net;
using BillingApplication.Abstractions;
using BillingApplication.Helpers;
using FluentAssertions;
using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.ExceptionExtensions;
using FluentValidation;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace BillingApplicationTests.Tests
{
    [TestClass]
    public class BillingControllerTests
    {
        private readonly IFixture _fixture;
        private readonly ILogger<BillingController> _logger;
        public BillingControllerTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoNSubstituteCustomization());
            _fixture.Customize(new BillingApplicationCustomization());
            _logger = _fixture.Create<ILogger<BillingController>>();
        }
        [TestMethod]
        public async Task When_ValidRequest_Then_ReturnOK()
        {
            //a           
            var fakeRequest = _fixture.Create<BillingRequest>();
            var fakeResponse = _fixture.Create<BillingResponse>();
            var token = new CancellationToken(false);           
            var fakeBillingService = _fixture.Create<IBillingService>();
            fakeBillingService.ProcessOrderAsync(fakeRequest, token).Returns(fakeResponse);          
            var billingController = new BillingController(_logger, fakeBillingService);          
            //aa
            var result = await billingController.PostOrder(fakeRequest, token);
            //aaa
            result.Should().BeOfType(typeof(OkObjectResult));
        }
        [TestMethod]
        public async Task When_NotValidRequest_Then_ReturnBadRequest()
        {
            //a           
            var fakeRequest = _fixture.Create<BillingRequest>();
            var exception = _fixture.Create<ValidationException>();
            var token = new CancellationToken(false);
            var fakeBillingService = _fixture.Create<IBillingService>();
            fakeBillingService.ProcessOrderAsync(fakeRequest, token).Throws(exception);
            var billingController = new BillingController(_logger, fakeBillingService);          
            
            //aa
            var result = await billingController.PostOrder(fakeRequest, token);
            
            //aaa
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }
        [TestMethod]
        public async Task When_ValidRequest_Then_ReturnUnprocessableEntity()
        {
            //a           
            var fakeRequest = _fixture.Create<BillingRequest>();
            var exception = _fixture.Create<JsonException>();
            var token = new CancellationToken(false);
            var fakeBillingService = _fixture.Create<IBillingService>();
            fakeBillingService.ProcessOrderAsync(fakeRequest, token).Throws(exception);
            var billingController = new BillingController(_logger, fakeBillingService);
            var expectedResult = billingController.StatusCode(StatusCodes.Status422UnprocessableEntity);
            
            //aa
            var result = await billingController.PostOrder(fakeRequest, token);
            
            //aaa
            result.Should().NotBeNull()
                 .And.BeEquivalentTo(expectedResult);
        }
        [TestMethod]
        public async Task When_ServiceFaulty_Then_ReturnBadRequest()
        {
            //a           
            var fakeRequest = _fixture.Create<BillingRequest>();
            var exception = _fixture.Create<NullReferenceException>();
            var token = new CancellationToken(false);
            var fakeBillingService = _fixture.Create<IBillingService>();
            fakeBillingService.ProcessOrderAsync(fakeRequest, token).Throws(exception);
            var billingController = new BillingController(_logger, fakeBillingService);
            
            //aa
            var result = await billingController.PostOrder(fakeRequest, token);
            
            //aaa
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }
    }
}
