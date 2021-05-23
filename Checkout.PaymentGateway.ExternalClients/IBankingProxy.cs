using System.Threading.Tasks;
using Checkout.PaymentGateway.Models.Core.Entities;
using Checkout.PaymentGateway.Models.Dtos;

namespace Checkout.PaymentGateway.ExternalClients
{
    public interface IBankingProxy
    {
        Task<BankResponse> ProcessPaymentRequest(Payment payment);
    }
}