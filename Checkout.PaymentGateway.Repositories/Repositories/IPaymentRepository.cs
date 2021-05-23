using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Models.Core.Entities;

namespace Checkout.PaymentGateway.Repositories.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> GetById(Guid id);

        Task<Payment> GetByIdIncludingBankCard(Guid id);

        Task<int> Save(Payment entity);
    }
}