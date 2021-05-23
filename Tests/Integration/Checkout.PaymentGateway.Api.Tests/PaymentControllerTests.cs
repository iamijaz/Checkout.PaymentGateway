using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Api.Tests.Helpers;
using Xunit;

namespace Checkout.PaymentGateway.Api.Tests
{
    public class PaymentControllerTests : IClassFixture<PaymentApplicationFactory<Startup>>,
        IClassFixture<IdentityServerApplicationFactory<IdentityServer.Startup>>
    {
        private readonly HttpClient _identityServerClient;

        private readonly HttpClient _paymentClient;

        private readonly ClientPaymentRequest _paymentRequest = new()
        {
            Amount = 10,
            Currency = "USD",
            PaymentCard = new PaymentCard
            {
                Name = "MR I",
                CardNumber = "4242424242424242",
                Cvv = "123",
                ExpiryMonth = DateTime.UtcNow.AddMonths(1).Month.ToString(),
                ExpiryYear = DateTime.UtcNow.AddYears(1).Year.ToString()
            }
        };

        public PaymentControllerTests(PaymentApplicationFactory<Startup> paymentAppFactory,
            IdentityServerApplicationFactory<IdentityServer.Startup> identityServerAppFactory)
        {
            _paymentClient = paymentAppFactory.CreateClient();
            _identityServerClient = identityServerAppFactory.CreateClient();
        }

        [Fact]
        public async Task WIth_Correct_Details_Payment_Request_Is_Processed_Successfully()
        {
            // Arrange Already done
            // Act 1- Create payment via POST
            var paymentPostResponse = await Post<ClientPaymentResponse>();

            // Act 2- Retrieve payment back via GET
            var paymentGetResponse =
                await Get<ClientPaymentResponse>(_paymentClient, paymentPostResponse.ResourceLinks[0].Href);

            // Assert
            paymentPostResponse.ResourceLinks = null; //To ignore links from the post operation
            Assert.Equal(JsonSerializer.Serialize(paymentPostResponse), JsonSerializer.Serialize(paymentGetResponse));
        }

        [Fact]
        public async Task With_Missing_Card_Details_Payment_Request_Is_Not_Processed()
        {
            // Arrange 
            _paymentRequest.PaymentCard = null;

            // Act
            var paymentPostResponse = await Post<ClientPaymentResponseErrors>(false);

            // Assert
            Assert.Single(paymentPostResponse.Errors.ToList());
        }

        [Fact]
        public async Task With_0_Amount_Payment_Request_Is_Not_Processed()
        {
            // Arrange 
            _paymentRequest.PaymentCard = null;

            // Act
            var paymentPostResponse = await Post<ClientPaymentResponseErrors>(false);

            // Assert
            Assert.Single(paymentPostResponse.Errors.ToList());
        }

        private static async Task<T> Get<T>(HttpClient client, string requestUri)
        {
            return await SubmitRequest<T>(() => client.GetAsync(requestUri));
        }

        private async Task<T> Post<T>(bool ensureSuccessStatusCode = true)
        {
            var token = await Get<string>(_identityServerClient, "api/v1/tokens");
            _paymentClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return await SubmitRequest<T>(() => _paymentClient.PostAsync("/api/v1/payments",
                    new StringContent(JsonSerializer.Serialize(_paymentRequest), Encoding.UTF8, "application/json")),
                ensureSuccessStatusCode);
        }

        private static async Task<T> SubmitRequest<T>(Func<Task<HttpResponseMessage>> action,
            bool ensureSuccessStatusCode = true)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var response = await action();

            // Must be successful.
            if (ensureSuccessStatusCode)
                response.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var contents = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(contents, options);
        }
    }
}