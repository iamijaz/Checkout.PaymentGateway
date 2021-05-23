namespace Checkout.PaymentGateway.Models.Dtos
{
    public class ResourceLink
    {
        public ResourceLink(string href, string rel, string type)
        {
            Href = href;
            Rel = rel;
            Type = type;
        }

        public string Href { get; set; }

        public string Rel { get; set; }

        public string Type { get; set; }
    }
}