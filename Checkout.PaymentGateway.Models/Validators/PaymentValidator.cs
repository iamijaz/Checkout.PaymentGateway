using Checkout.PaymentGateway.Models.Core.Entities;
using FluentValidation;

namespace Checkout.PaymentGateway.Models.Validators
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
        public PaymentValidator()
        {
            AddAmountValidation();
            AddCurrencyValidation();
            AddBankCardValidation();
        }

        private void AddBankCardValidation()
        {
            RuleFor(payment => payment.BankCard)
                .NotNull()
                .WithMessage("Bank Card details can't be null")
                .SetValidator(new BankCardValidator());
        }

        private void AddAmountValidation()
        {
            RuleFor(payment => payment.Amount)
                .GreaterThan(0)
                .WithMessage("Amount needs to be greater than 0");
        }

        private void AddCurrencyValidation()
        {
            RuleFor(payment => payment.Currency)
                .NotNull()
                .WithMessage("Currency must be specified");
        }
    }
}