using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Models.Validators;
using FluentValidation.Results;

namespace Checkout.PaymentGateway.Models.Core.Entities
{
    public class BankCard : Entity
    {
        private BankCard()
        {
        }

        public BankCard(string name, string cardNumber, string expiryMonth, string expiryYear, string cvv)
        {
            Id = Guid.NewGuid();
            Name = name;
            CardNumber = cardNumber;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            CVV = cvv;
        }

        public string CVV { get; private set; }

        public string Name { get; private set; }

        public string CardNumber { get; private set; }

        public string ExpiryMonth { get; private set; }

        public string ExpiryYear { get; private set; }

        public override Task<ValidationResult> Validate()
        {
            return new BankCardValidator().ValidateAsync(this);
        }
    }
}