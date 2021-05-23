using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Models.Dtos;

namespace Checkout.PaymentGateway.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponse> Pay(PaymentRequest paymentRequest);

        Task<PaymentResponse> Get(Guid id);
    }
}