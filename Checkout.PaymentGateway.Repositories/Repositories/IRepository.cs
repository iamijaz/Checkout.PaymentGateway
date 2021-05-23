using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Models.Core.Entities;

namespace Checkout.PaymentGateway.Repositories.Repositories
{
    public interface IRepository<T>
        where T : Entity
    {
        Task<T> GetById(Guid id);

        Task<int> Save(T entity);
    }
}