namespace Checkout.PaymentGateway.Api.Tests.Helpers
{
    public class ClientPaymentResponse
    {
        public ResourceLink[] ResourceLinks { get; set; }
        public string Id { get; set; }
        public ClientPaymentRequest Payment { get; set; }
        public string PaymentStatus { get; set; }
    }

    public class PaymentCard
    {
        public string Cvv { get; set; }
        public string Name { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
    }

    public class ResourceLink
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Type { get; set; }
    }

    public class ClientPaymentRequest
    {
        public int Amount { get; set; }
        public string Currency { get; set; }
        public PaymentCard PaymentCard { get; set; }
    }


    public class ClientPaymentResponseErrors
    {
        public Error[] Errors { get; set; }
        public string Id { get; set; }
        public ClientPaymentRequest Payment { get; set; }
        public string PaymentStatus { get; set; }
    }

    public class Error
    {
        public string Message { get; set; }
    }
}