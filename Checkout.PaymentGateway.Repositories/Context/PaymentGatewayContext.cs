using Checkout.PaymentGateway.Models.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Checkout.PaymentGateway.Repositories.Context
{
    public class PaymentGatewayContext : DbContext
    {
        public PaymentGatewayContext(DbContextOptions<PaymentGatewayContext> options)
            : base(options)
        {
        }

        public DbSet<Payment> Payment { get; set; }
    }
}