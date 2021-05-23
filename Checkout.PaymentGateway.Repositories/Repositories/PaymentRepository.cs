using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Models.Core.Entities;
using Checkout.PaymentGateway.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace Checkout.PaymentGateway.Repositories.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(PaymentGatewayContext context) :
            base(context)
        {
        }

        public Task<Payment> GetByIdIncludingBankCard(Guid id)
        {
            return Context.Set<Payment>().Include(p => p.BankCard).FirstOrDefaultAsync(p => p.Id.Equals(id));
        }
    }
}