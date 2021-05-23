using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Models.Core.Entities;
using Checkout.PaymentGateway.Repositories.Context;
using Checkout.PaymentGateway.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Checkout.PaymentGateway.Repositories.Tests
{
    public class PaymentRepositoryTests
    {
        private readonly Payment _payment = new(10, "USD", new BankCard("MR I", "4242424242424242",
            DateTime.UtcNow.AddMonths(1).Month.ToString(),
            DateTime.UtcNow.AddYears(1).Year.ToString(), "123"));

        [Fact]
        public async Task With_A_Valid_Payment_Request_Details_Are_Persisted_And_Retrieved_Successfully()
        {
            // Arrange
            var dbContextOptions = await SaveInTheDb();

            // Act
            await using var context = new PaymentGatewayContext(dbContextOptions);
            var sut = new PaymentRepository(context);

            // Assert
            var paymentFromDatabase = await sut.GetByIdIncludingBankCard(_payment.Id);
            Assert.NotNull(paymentFromDatabase);
            Assert.Equal(_payment.Amount, paymentFromDatabase.Amount);
            Assert.Equal(_payment.Currency, paymentFromDatabase.Currency);
            Assert.NotNull(paymentFromDatabase.BankCard);

            var cardFromDb = paymentFromDatabase.BankCard;

            var bankCard = new BankCard("MR I", "4242424242424242", DateTime.UtcNow.AddMonths(1).Month.ToString(),
                DateTime.UtcNow.AddYears(1).Year.ToString(), "123");
            Assert.Equal(bankCard.CVV, cardFromDb?.CVV);
            Assert.Equal(bankCard.Name, cardFromDb?.Name);
            Assert.Equal(bankCard.CardNumber, cardFromDb?.CardNumber);
            Assert.Equal(bankCard.ExpiryMonth, cardFromDb?.ExpiryMonth);
            Assert.Equal(bankCard.ExpiryYear, cardFromDb?.ExpiryYear);
        }

        private async Task<DbContextOptions<PaymentGatewayContext>> SaveInTheDb()
        {
            var dbContextOptions = new DbContextOptionsBuilder<PaymentGatewayContext>()
                .UseInMemoryDatabase("GetWithBankCardTestDatabase")
                .Options;

            // act
            await using var context = new PaymentGatewayContext(dbContextOptions);
            var repository = new PaymentRepository(context);
            await repository.Save(_payment);

            return dbContextOptions;
        }
    }
}