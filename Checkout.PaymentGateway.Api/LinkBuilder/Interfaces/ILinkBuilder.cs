namespace Checkout.PaymentGateway.Api.LinkBuilder.Interfaces
{
    public interface ILinkBuilder<TSource>
    {
        TSource BuildLinks(TSource source);
    }
}