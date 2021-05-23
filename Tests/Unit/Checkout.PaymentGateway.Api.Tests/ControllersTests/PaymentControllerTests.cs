using System;
using System.Net;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Api.Controllers;
using Checkout.PaymentGateway.Api.LinkBuilder.Interfaces;
using Checkout.PaymentGateway.Models.Dtos;
using Checkout.PaymentGateway.Models.Enums;
using Checkout.PaymentGateway.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Checkout.PaymentGateway.Api.Tests.ControllersTests
{
    public class PaymentControllerTests
    {
        private readonly PaymentRequest _paymentRequest = new()
        {
            Amount = 10,
            Currency = "USD",
            PaymentCard = new PaymentCard(
                "MR I",
                "4242424242424242",
                DateTime.UtcNow.AddMonths(1).Month.ToString(),
                DateTime.UtcNow.AddYears(1).Year.ToString(),
                "123"
            )
        };

        // System Under Test
        private readonly PaymentController _sut;
        private readonly Mock<IPaymentResponseLinkBuilder> _paymentResponseLinkBuilder;
        private readonly Mock<IPaymentService> _paymentService;

        public PaymentControllerTests()
        {
            _paymentService = new Mock<IPaymentService>();
            _paymentResponseLinkBuilder = new Mock<IPaymentResponseLinkBuilder>();

            _sut = new PaymentController(_paymentService.Object, _paymentResponseLinkBuilder.Object);
        }

        [Fact]
        public async Task With_Null_Payment_Request_BadRequest_Is_Returned()
        {
            // Arrange
            PaymentRequest paymentRequest = null;

            // Act
            var actionResult = await _sut.Post(paymentRequest);

            // Assert
            Assert.True(actionResult.Result is BadRequestObjectResult);
            Assert.Equal((actionResult.Result as BadRequestObjectResult)?.StatusCode, (int)HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task With_A_Valid_Payment_Request_Successful_Response_Is_Returned()
        {
            // Arrange
            var paymentResponseSuccess = new PaymentResponseSuccess();

            _paymentService
                .Setup(p => p.Pay(It.IsAny<PaymentRequest>()))
                .ReturnsAsync(paymentResponseSuccess);

            _paymentResponseLinkBuilder
                .Setup(p => p.BuildLinks(It.IsAny<PaymentResponseSuccess>()))
                .Returns(paymentResponseSuccess);

            // Act
            var actionResult = await _sut.Post(_paymentRequest);

            // Assert
            Assert.True(actionResult.Result is OkObjectResult);
            Assert.Equal((actionResult.Result as OkObjectResult)?.StatusCode, (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task With_An_Invalid_Payment_Request_BadRequest_Is_Returned()
        {
            // Arrange
            _paymentRequest.PaymentCard.CardNumber = string.Empty;
            var paymentResponseFailed = new PaymentResponseFailed();

            _paymentService
                .Setup(p => p.Pay(It.IsAny<PaymentRequest>()))
                .ReturnsAsync(paymentResponseFailed);

            // Act
            var actionResult = await _sut.Post(_paymentRequest);

            // Assert
            Assert.True(actionResult.Result is BadRequestObjectResult);
            Assert.Equal((actionResult.Result as BadRequestObjectResult)?.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task With_An_Invalid_Payment_Request_Id_NotFound_Is_Returned()
        {
            // Arrange
            var id = default(Guid);
            var paymentResponseFailed = new PaymentResponseFailed();

            _paymentService
                .Setup(p => p.Get(It.IsAny<Guid>()))
                .ReturnsAsync(paymentResponseFailed);

            // Act
            var actionResult = await _sut.Get(id.ToString());

            // Assert
            Assert.True(actionResult.Result is NotFoundObjectResult);
            Assert.Equal((actionResult.Result as NotFoundObjectResult)?.StatusCode, (int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task With_A_Valid_PaymentId_Successful_Response_Is_Returned()
        {
            // Arrange
            var id = Guid.NewGuid();
            var paymentResponseSuccess =
                new PaymentResponseSuccess(id, _paymentRequest, PaymentProcessingStatus.Success);

            _paymentService
                .Setup(p => p.Get(It.IsAny<Guid>()))
                .ReturnsAsync(paymentResponseSuccess);

            // Act
            var actionResult = await _sut.Get(id.ToString());

            // Assert
            Assert.True(actionResult.Result is OkObjectResult);

            var okResult = actionResult.Result as OkObjectResult;
            var resultValue = okResult?.Value as PaymentResponseSuccess;

            Assert.Equal(okResult?.StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal(_paymentRequest.Amount, resultValue?.Payment.Amount);
            Assert.Equal(_paymentRequest.Currency, resultValue?.Payment.Currency);
            Assert.Equal(_paymentRequest.PaymentCard.CardNumber, resultValue?.Payment.PaymentCard.CardNumber);
            Assert.Equal(_paymentRequest.PaymentCard.Name, resultValue?.Payment.PaymentCard.Name);
            Assert.Equal(_paymentRequest.PaymentCard.CVV, resultValue?.Payment.PaymentCard.CVV);
        }
    }
}