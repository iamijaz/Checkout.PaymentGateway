using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.Api.Tests.Helpers
{
    public class IdentityServerApplicationFactory<TStartup> : WebApplicationFactory<IdentityServer.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .BuildServiceProvider();
            });
        }
    }
}