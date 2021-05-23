namespace Checkout.PaymentGateway.Models.Dtos
{
    public class PaymentCard
    {
        public PaymentCard(string name, string cardNumber, string expiryMonth, string expiryYear, string cvv)
        {
            Name = name;
            CardNumber = cardNumber;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            CVV = cvv;
        }

        public string CVV { get; set; }

        public string Name { get; set; }

        public string CardNumber { get; set; }

        public string ExpiryMonth { get; set; }

        public string ExpiryYear { get; set; }
    }
}