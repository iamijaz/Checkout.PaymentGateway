using System.Text.RegularExpressions;

namespace Checkout.PaymentGateway.Models.Dtos
{
    public class SecureCardInformation
    {
        public static PaymentCard MaskCardSensibleInformation(PaymentCard paymentCard)
        {
            var cvvRegex = new Regex(".", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var cardNumberRegex = new Regex(".(?=.{4})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var cvvMasked = cvvRegex.Replace(paymentCard.CVV, "X");

            var cardNumberMasked = cardNumberRegex.Replace(paymentCard.CardNumber, "X");

            return new PaymentCard(paymentCard.Name, cardNumberMasked, paymentCard.ExpiryMonth, paymentCard.ExpiryYear,
                cvvMasked);
        }
    }
}