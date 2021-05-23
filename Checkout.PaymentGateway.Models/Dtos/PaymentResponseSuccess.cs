using System;
using System.Collections.Generic;
using Checkout.PaymentGateway.Models.Enums;

namespace Checkout.PaymentGateway.Models.Dtos
{
    public class PaymentResponseSuccess : PaymentResponse
    {
        public PaymentResponseSuccess(Guid id, PaymentRequest payment, PaymentProcessingStatus paymentPaymentStatus)
        {
            Id = id;
            Payment = payment;
            PaymentStatus = paymentPaymentStatus.ToString();
        }

        public PaymentResponseSuccess()
        {
            ResourceLinks = new List<ResourceLink>();
        }

        public IList<ResourceLink> ResourceLinks { get; set; }
    }
}