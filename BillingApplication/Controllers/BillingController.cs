using BillingApplication.Abstractions;
using BillingApplication.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BillingApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BillingController : ControllerBase
    {
        private readonly ILogger<BillingController> _logger;
        private readonly IBillingService _billingService;
        public BillingController(ILogger<BillingController> logger, IBillingService billingService)
        {
            _logger = logger;
            _billingService = billingService;
        }       
       
        [HttpPost(Name = "PostOrder")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillingResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> PostOrder(BillingRequest request, CancellationToken token = default)
        {
            try
            {
                this._logger.LogInformation($"Processing order request: \r\nOrderNumber: {request.OrderNumber} \r\nUserId: {request.UserId} \r\nGatewayId: {request.PaymentGatewayId}");
                var res = await _billingService.ProcessOrderAsync(request, token);
                this._logger.LogInformation($"Request OK");
                return Ok(res);
            }
            catch(ValidationException vEx)
            {
                this._logger.LogError($"Validation exception: {vEx.Message}");
                return BadRequest(vEx.Message);
            }
            catch(JsonException jEx)
            {
                this._logger.LogError($"Response parsing exception: {jEx.Message}");

                return StatusCode(StatusCodes.Status422UnprocessableEntity);
            }
            catch(Exception ex)
            {
                this._logger.LogError($"General exception: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}