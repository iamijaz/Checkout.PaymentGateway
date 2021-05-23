using System.Collections.Generic;

namespace Checkout.PaymentGateway.Models.Dtos
{
    public class PaymentResponseFailed : PaymentResponse
    {
        public PaymentResponseFailed()
        {
        }

        public PaymentResponseFailed(IEnumerable<PaymentErrors> errors)
        {
            Errors = errors;
        }

        public IEnumerable<PaymentErrors> Errors { get; set; }
    }
}