using AutoMapper;
using Checkout.PaymentGateway.Models.Core.Entities;
using Checkout.PaymentGateway.Models.Dtos;

namespace Checkout.PaymentGateway.Models
{
    public class PaymentAutoMapper : Profile
    {
        public PaymentAutoMapper()
        {
            CreateMap<BankCard, PaymentCard>()
                .ConvertUsing(source => ToBankCardDto(source));
            CreateMap<PaymentCard, BankCard>()
                .ConvertUsing(source => ToBankCard(source));

            CreateMap<Payment, PaymentRequest>()
                .ForMember(d => d.PaymentCard, x => x.MapFrom(s => s.BankCard));
            CreateMap<PaymentRequest, Payment>()
                .ForMember(d => d.BankCard, x => x.MapFrom(s => s.PaymentCard));
        }

        private static PaymentCard ToBankCardDto(BankCard source)
        {
            return source switch
            {
                null => null,
                _ => SecureCardInformation.MaskCardSensibleInformation(new PaymentCard(source.Name,
                    source.CardNumber, source.ExpiryMonth, source.ExpiryYear, source.CVV))
            };
        }

        private static BankCard ToBankCard(PaymentCard source)
        {
            return source switch
            {
                null => null,
                _ => new BankCard(source.Name, source.CardNumber, source.ExpiryMonth, source.ExpiryYear, source.CVV)
            };
        }
    }
}