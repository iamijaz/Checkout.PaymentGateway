using System.Threading.Tasks;
using FluentValidation.Results;

namespace Checkout.PaymentGateway.Models.Validators
{
    public interface IValidator
    {
        Task<ValidationResult> Validate();
    }
}