namespace Checkout.PaymentGateway.Models.Dtos
{
    public class PaymentErrors
    {
        public PaymentErrors(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}