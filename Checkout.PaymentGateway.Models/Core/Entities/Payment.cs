using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Models.Enums;
using Checkout.PaymentGateway.Models.Validators;
using FluentValidation.Results;

namespace Checkout.PaymentGateway.Models.Core.Entities
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    public class Payment : Entity
    {
        private Payment()
        {
        }

        public Payment(decimal amount, string currency, BankCard bankCard)
        {
            Id = Guid.NewGuid();
            Amount = amount;
            Currency = currency;
            BankCard = bankCard;

            CreatedAt = DateTime.UtcNow;
            ProcessingStatus = PaymentProcessingStatus.Pending;
        }

        public decimal Amount { get; private set; }

        public string Currency { get; private set; }

        public PaymentProcessingStatus ProcessingStatus { get; private set; }

        public Guid BankProcessingId { get; private set; }

        public BankCard BankCard { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public void Processing()
        {
            ProcessingStatus = PaymentProcessingStatus.Processing;
        }

        public void Pending()
        {
            ProcessingStatus = PaymentProcessingStatus.Pending;
        }

        public void Rejected()
        {
            ProcessingStatus = PaymentProcessingStatus.Rejected;
        }

        public void Success()
        {
            ProcessingStatus = PaymentProcessingStatus.Success;
        }

        public override Task<ValidationResult> Validate()
        {
            return new PaymentValidator().ValidateAsync(this);
        }

        public void SetBankResponseId(Guid bankProcessingId)
        {
            BankProcessingId = bankProcessingId;
        }
    }
}