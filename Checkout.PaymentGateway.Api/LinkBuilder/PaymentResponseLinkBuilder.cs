using System.Collections.Generic;
using Checkout.PaymentGateway.Api.Controllers;
using Checkout.PaymentGateway.Api.LinkBuilder.Interfaces;
using Checkout.PaymentGateway.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.Api.LinkBuilder
{
    public class PaymentResponseLinkBuilder : IPaymentResponseLinkBuilder
    {
        private readonly IUrlHelper _urlHelper;

        public PaymentResponseLinkBuilder(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public PaymentResponseSuccess BuildLinks(PaymentResponseSuccess source)
        {
            var idObj = new {id = source.Id};

            source.ResourceLinks = new List<ResourceLink>
            {
                new(_urlHelper.Link(nameof(PaymentController.Get), idObj),
                    "payments",
                    "GET")
            };


            return source;
        }
    }
}