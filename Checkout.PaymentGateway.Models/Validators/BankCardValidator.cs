using System;
using Checkout.PaymentGateway.Models.Core.Entities;
using FluentValidation;

namespace Checkout.PaymentGateway.Models.Validators
{
    public class BankCardValidator : AbstractValidator<BankCard>
    {
        public BankCardValidator()
        {
            AddCvvValidation();
            AddNumberValidation();
            AddNameValidation();
            AddMonthExpiryDateValidation();
            AddYearExpiryDateValidation();
        }

        private void AddCvvValidation()
        {
            RuleFor(card => card.CVV)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty()
                .WithMessage("CVV can not be null or empty")
                .Must(cvv => cvv.Length is >= 3 and <= 4)
                .WithMessage("CVV Must have at least 3 digits and less than 4 digits")
                .Must(cvv => int.TryParse(cvv, out var cvvNumber))
                .WithMessage("CVV Must be a valid number");
        }

        private void AddNumberValidation()
        {
            RuleFor(card => card.CardNumber)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty()
                .WithMessage("Card number can not be null or empty")
                .Must(number => decimal.TryParse(number, out var num))
                .WithMessage("Card number must be a valid number")
                .MinimumLength(14)
                .WithMessage("Car number must have more than 14 digits")
                .MaximumLength(20)
                .WithMessage("Car number must have less than 20 digits");
        }

        private void AddNameValidation()
        {
            RuleFor(card => card.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEmpty()
                .WithMessage("Name can't be null or empty")
                .MinimumLength(1)
                .WithMessage("Name must be greater than 0 characters")
                .MaximumLength(255)
                .WithMessage("Name must be smaller than 255 characters");
        }

        public void AddMonthExpiryDateValidation()
        {
            RuleFor(e => e.ExpiryMonth)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty()
                .WithMessage("ExpiryMonth can't not be null or empty")
                .Must(m => int.TryParse(m, out var month))
                .WithMessage("Expiry date month must be a valid integer")
                .Must(m => int.Parse(m) >= DateTime.UtcNow.Month)
                .WithMessage("Expiry date month must be greater or equal to the current month");
        }

        public void AddYearExpiryDateValidation()
        {
            RuleFor(e => e.ExpiryYear)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty()
                .WithMessage("ExpiryYear can't not be null or empty")
                .Must(y => int.TryParse(y, out var month))
                .WithMessage("Expiry date year must be a valid integer")
                .Must(y => int.Parse(y) >= DateTime.UtcNow.Year)
                .WithMessage("Expiry date year must be greater or equal to the current year");
        }
    }
}