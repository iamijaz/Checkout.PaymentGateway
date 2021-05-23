using System;
using Checkout.PaymentGateway.Models.Enums;

namespace Checkout.PaymentGateway.Models.Dtos
{
    public class BankResponse
    {
        public Guid BankProcessId { get; set; }

        public PaymentProcessingStatus Status { get; set; }
    }
}