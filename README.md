
# Checkout PaymentGateway Architecture

The solution contains .NET core PaymentGateway API which is structured into the following components:

[Architecture]
![Checkout.PaymentGateway Architecture](/Diagrams/Architecture.png) 

* **Payment API** (Checkout.PaymentGateway.Api)- Contains the core payment services for both Making and Retrieving a payment.
* **Token Service** (Checkout.PaymentGateway.IdentityServer)- Contains one main function, generated token on demand. Just to simulate an Identity Server.
* **Bank Service** (Checkout.PaymentGateway.ExternalClients)- This is a stub proxy service, Just to simulate the banking operations.
* **Swagger** Both Token service and Payment API has contemporary Swagger, for an easy play around.

## General Flow

As per the diagram above, generally flow works as following:
1. A token is requested from the IdentityServer.
2. To make a payment request to the Payment Service, a token is passed along as an authorization header.
3. Payment Service connects to the Banking Service and validates the payment.
4. Payment Service is also persisting the payment details. (Currently using EF Core InMemory DB but which can easily be extended into a production DB.

#  Setting Up and Running the Application

1. **Prerequisite:** .Net 5.0 and C# 9.0
2.  Application can be run in two modes
	1. **In a local server eg Kestrel** via Visual Studio or dotnet CLI
	2. **In Docker Mode** Via docker-compose up
		[docker-compose up]
![docker-compose up](/Diagrams/dockerup.png) 
3. Once the Application is running, two swagger browsers windows will open
[docker running]
![docker running](/Diagrams/dockerunning.png) 
4. One for the Payment API
5. Other for the Token API
	[Identity Server Swagger]
![IdentityServer Swagger](/Diagrams/IdServer.png) 
6. Create a token and setup at Payment API via Authorization click e.g. Bearer [Token]
7. Open the Payment Post endpoint and supply the payment details, following sample could be used
8. Once the Response is received use it with the Get endpoint to retrieve the payment details.
	[Payment Service Swagger]
![Payment Service Swagger](/Diagrams/PaymentService.png) 
9.  Sample Request Details :
```
{
"amount": 100,
"currency": "USD",
"paymentCard": {
"cvv": "123",
"name": "MR I",
"cardNumber": "4242424242424242",
"expiryMonth": "10",
"expiryYear": "2025"
 }
}
```
10.  Sample Response:
```
{
"resourceLinks": [
{
"href": "https://localhost:5002/api/v1/payments/4ee32444-d645-4ddd-b513-9be73332b936",
"rel": "payments",
"type": "GET"
 }
],
"id": "4ee32444-d645-4ddd-b513-9be73332b936",
"payment": {
"amount": 100,
"currency": "USD",
"paymentCard": {
"cvv": "XXX",
"name": "MR I",
"cardNumber": "XXXXXXXXXXXX4242",
"expiryMonth": "10",
"expiryYear": "2025"
  }
 },
"paymentStatus": "Pending"
}
```


## Features
1. Supports the versioning and HATEOAS Driven RESTful API.
2. Contemporary Swagger based interactive documentation.
3. EF Core support.
4. Authentication and Tokens Authorizations.
5. Docker and Containerization configured.
6. Rich set of Integrations and Unite Tests suites, which tests the whole application inside out.
[Tests suites]
![Tests suites](/Diagrams/tests.png) 
## Improvements and Assumptions
1. The main point here is proof of concepts and flow is solid. All the production architectural significant components e.g. Acquiring Bank interface, Identity server, Storage, Containerization etc are assumed and confgiured at a basic working level.
2. Although the Logging interface is configured but hasn't been used.
3. More Unit and Integration tests could have been added.
4. Code reviews and peer reviews can help improve code quality.
5. Working closely with domain owners help using the rich domain vocabulary and concepts.

# Glossary
1. **Payment API:** A web-based interface for payment processing.
2. **Token Service** A mock-up server to retrieve authentication tokens.
3. **Bank Service** To mimic the Acquiring Bank services.
4. **EF Core** .Net Entity Framework for database storage operations.
5. **Swagger** A tool for documenting and interacting with Web APIs.
