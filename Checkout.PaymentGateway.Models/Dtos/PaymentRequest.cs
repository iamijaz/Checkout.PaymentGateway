namespace Checkout.PaymentGateway.Models.Dtos
{
    public class PaymentRequest
    {
        public PaymentRequest()
        {
        }

        public PaymentRequest(
            decimal amount,
            string currency,
            PaymentCard paymentCard)
        {
            Amount = amount;
            Currency = currency;
            PaymentCard = paymentCard;
        }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public PaymentCard PaymentCard { get; set; }
    }
}