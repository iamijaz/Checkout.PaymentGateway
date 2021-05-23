using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Api.LinkBuilder.Interfaces;
using Checkout.PaymentGateway.Models.Dtos;
using Checkout.PaymentGateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentResponseLinkBuilder _paymentResponseLinkBuilder;
        private readonly IPaymentService _paymentService;

        public PaymentController(
            IPaymentService paymentService,
            IPaymentResponseLinkBuilder paymentResponseLinkBuilder)
        {
            _paymentService = paymentService;
            _paymentResponseLinkBuilder = paymentResponseLinkBuilder;
        }

        /// <summary>
        ///     Post a payment request
        /// </summary>
        /// <param name="paymentRequest">Request for a payment</param>
        /// <returns>
        ///     If success, contains the possible links for the next actions
        /// </returns>
        /// <response code="200"></response>
        /// <response code="400"></response>
        [HttpPost(Name = nameof(Post))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PaymentResponse>> Post(PaymentRequest paymentRequest)
        {
            return paymentRequest switch
            {
                null => BadRequest(new { Message = $"Parameter {nameof(PaymentRequest)} can't be null" }),
                _ => await CreatePayment(paymentRequest)
            };
        }

        private async Task<ActionResult<PaymentResponse>> CreatePayment(PaymentRequest paymentRequest)
        {
            var response = await _paymentService.Pay(paymentRequest);
            return response switch
            {
                PaymentResponseSuccess dto => Ok(_paymentResponseLinkBuilder.BuildLinks(dto)),
                _ => BadRequest(response)
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="id">Guid format</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404"></response>
        [Route("{id}", Name = nameof(Get))]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PaymentResponse>> Get(string id)
        {
            return Guid.TryParse(id, out var paymentId) switch
            {
                false => BadRequest(new { Message = $"Parameter {nameof(id)} can't be blank or null" }),
                true => await GetPayment(paymentId)
            };
        }

        private async Task<ActionResult<PaymentResponse>> GetPayment(Guid paymentId)
        {
            var response = await _paymentService.Get(paymentId);
            return response switch
            {
                PaymentResponseFailed _ => NotFound(response),
                _ => Ok(response)
            };
        }
    }
}