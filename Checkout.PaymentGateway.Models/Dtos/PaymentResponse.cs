using System;

namespace Checkout.PaymentGateway.Models.Dtos
{
    public abstract class PaymentResponse
    {
        public Guid Id { get; set; }

        public PaymentRequest Payment { get; set; }

        public string PaymentStatus { get; set; }
    }
}