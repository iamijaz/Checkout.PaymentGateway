using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.Api.Ioc
{
    public static class DependencyInjectionBootstrap
    {
        public static IServiceCollection Setup(
            IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment env)
        {
            services
                .AddServices()
                .AddRepositories(configuration, env);

            return services;
        }
    }
}