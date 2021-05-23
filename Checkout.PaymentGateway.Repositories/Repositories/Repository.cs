using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Models.Core.Entities;
using Checkout.PaymentGateway.Repositories.Context;

namespace Checkout.PaymentGateway.Repositories.Repositories
{
    public abstract class Repository<T> : IRepository<T>
        where T : Entity
    {
        protected readonly PaymentGatewayContext Context;

        protected Repository(PaymentGatewayContext context)
        {
            Context = context;
        }

        public Task<int> Save(T entity)
        {
            Context.Set<T>().Add(entity);
            return Context.SaveChangesAsync();
        }

        public Task<T> GetById(Guid id)
        {
            return Context.Set<T>().FindAsync(id).AsTask();
        }
    }
}