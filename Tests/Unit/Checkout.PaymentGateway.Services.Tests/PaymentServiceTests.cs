using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.PaymentGateway.ExternalClients;
using Checkout.PaymentGateway.Models;
using Checkout.PaymentGateway.Models.Core.Entities;
using Checkout.PaymentGateway.Models.Dtos;
using Checkout.PaymentGateway.Models.Enums;
using Checkout.PaymentGateway.Repositories.Repositories;
using Moq;
using Xunit;

namespace Checkout.PaymentGateway.Services.Tests
{
    public class PaymentServiceTests
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
        private readonly PaymentService _sut;
        private readonly Mock<IBankingProxy> _bankingProxyMock;
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;

        public PaymentServiceTests()
        {
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new PaymentAutoMapper()); });
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _bankingProxyMock = new Mock<IBankingProxy>();

            _sut = new PaymentService(
                _paymentRepositoryMock.Object,
                _bankingProxyMock.Object,
                mappingConfig.CreateMapper()
            );
        }

        [Fact]
        public async Task With_A_Valid_Payment_Request_Successful_Response_Is_Returned2()
        {
            // Arrange
            _paymentRepositoryMock
                .Setup(p =>
                    p.Save(It.IsAny<Payment>()))
                .ReturnsAsync(1);

            _bankingProxyMock
                .Setup(b =>
                    b.ProcessPaymentRequest(It.IsAny<Payment>()))
                .ReturnsAsync(new BankResponse
                {
                    Status = PaymentProcessingStatus.Success
                });

            // Act
            var result = await _sut.Pay(_paymentRequest);

            // Assert
            Assert.NotNull(result);
            Assert.True(result is PaymentResponseSuccess);
        }

        [Fact]
        public async Task With_An_Invalid_Payment_Request_Successful_Response_Is_Returned()
        {
            // Arrange
            _paymentRequest.PaymentCard.CardNumber = string.Empty;
            _paymentRepositoryMock
                .Setup(p =>
                    p.Save(It.IsAny<Payment>()))
                .ReturnsAsync(1);

            _bankingProxyMock
                .Setup(b =>
                    b.ProcessPaymentRequest(It.IsAny<Payment>()))
                .ReturnsAsync(new BankResponse
                {
                    Status = PaymentProcessingStatus.Success
                });

            // Act
            var result = await _sut.Pay(_paymentRequest);

            // Assert
            Assert.NotNull(result);
            Assert.True(result is PaymentResponseFailed);

            var paymentResponse = result as PaymentResponseFailed;

            Assert.NotNull(paymentResponse.Errors);
            Assert.True(paymentResponse.Errors.Any());
        }

        [Fact]
        public async Task With_A_Valid_Payment_Id_Request_Successful_Response_Is_Returned()
        {
            // Arrange
            Payment payment = new(10, "USD", new BankCard("MR I", "4242424242424242",
                DateTime.UtcNow.AddMonths(1).Month.ToString(),
                DateTime.UtcNow.AddYears(1).Year.ToString(), "123"));

            _paymentRepositoryMock
                .Setup(p =>
                    p.GetByIdIncludingBankCard(It.Is<Guid>(x => x == payment.Id)))
                .ReturnsAsync(payment);

            // Act
            var result = await _sut.Get(payment.Id);

            // Assert
            Assert.NotNull(result);
            Assert.True(result is PaymentResponseSuccess);

            var paymentResponse = result as PaymentResponseSuccess;

            Assert.Equal(payment.Id, paymentResponse.Id);
            Assert.NotNull(paymentResponse.Payment);
            Assert.Equal(payment.Amount, paymentResponse.Payment.Amount);
            Assert.Equal(payment.Currency, paymentResponse.Payment.Currency);
            Assert.NotNull(paymentResponse.Payment.PaymentCard);

            var bankCard = paymentResponse.Payment.PaymentCard;

            Assert.Equal("XXX", bankCard.CVV);
            Assert.Equal(payment.BankCard.Name, bankCard.Name);
            Assert.Equal("XXXXXXXXXXXX4242", bankCard.CardNumber);
            Assert.Equal(payment.BankCard.ExpiryMonth, bankCard.ExpiryMonth);
            Assert.Equal(payment.BankCard.ExpiryYear, bankCard.ExpiryYear);
        }
    }
}