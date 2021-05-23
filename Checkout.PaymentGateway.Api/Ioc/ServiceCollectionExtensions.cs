using Checkout.PaymentGateway.Api.LinkBuilder;
using Checkout.PaymentGateway.Api.LinkBuilder.Interfaces;
using Checkout.PaymentGateway.ExternalClients;
using Checkout.PaymentGateway.Repositories.Context;
using Checkout.PaymentGateway.Repositories.Repositories;
using Checkout.PaymentGateway.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Checkout.PaymentGateway.Api.Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiDependencies(this IServiceCollection services)
        {
            // Add builder
            services.AddScoped<IPaymentResponseLinkBuilder, PaymentResponseLinkBuilder>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration,
            IWebHostEnvironment env)
        {
            // Add repository
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            // Adding Db Context
            if (env.IsDevelopment())
                services
                    .AddDbContext<PaymentGatewayContext>(
                        options => options.UseInMemoryDatabase("InMemoryPaymentGatewayDataBase"));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Add services
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IBankingProxy, BankingProxy>();

            return services;
        }
    }
}