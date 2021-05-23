using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Models.Validators;
using FluentValidation.Results;

namespace Checkout.PaymentGateway.Models.Core.Entities
{
    public abstract class Entity : IValidator
    {
        public Guid Id { get; protected set; }

        public abstract Task<ValidationResult> Validate();
    }
}