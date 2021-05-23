using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Models.Core.Entities;
using Checkout.PaymentGateway.Models.Dtos;
using Checkout.PaymentGateway.Models.Enums;

namespace Checkout.PaymentGateway.ExternalClients
{
    public class BankingProxy : IBankingProxy
    {
        public Task<BankResponse> ProcessPaymentRequest(Payment payment)
        {
            var bankResponse = new BankResponse { BankProcessId = Guid.NewGuid() };

            payment.SetBankResponseId(bankResponse.BankProcessId);
            return GetRandomPaymentProcessingStatus() switch
            {
                PaymentProcessingStatus.Pending => PaymentPending(payment, bankResponse),
                PaymentProcessingStatus.Processing => PaymentProcessing(payment, bankResponse),
                PaymentProcessingStatus.Rejected => PaymentFailed(payment, bankResponse),
                PaymentProcessingStatus.Success => PaymentSucceeded(payment, bankResponse),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static PaymentProcessingStatus GetRandomPaymentProcessingStatus()
        {
            var statusArray = Enum.GetValues<PaymentProcessingStatus>();
            var random = new Random();
            var selectedStatus = statusArray[random.Next(statusArray.Length)];
            return selectedStatus;
        }

        private static Task<BankResponse> PaymentProcessing(Payment payment, BankResponse bankResponse)
        {
            bankResponse.Status = PaymentProcessingStatus.Processing;
            payment.Processing();
            return Task.FromResult(bankResponse);
        }

        private static Task<BankResponse> PaymentPending(Payment payment, BankResponse bankResponse)
        {
            bankResponse.Status = PaymentProcessingStatus.Pending;
            payment.Pending();
            return Task.FromResult(bankResponse);
        }

        private static Task<BankResponse> PaymentSucceeded(Payment payment, BankResponse bankResponse)
        {
            bankResponse.Status = PaymentProcessingStatus.Success;
            payment.Success();
            return Task.FromResult(bankResponse);
        }

        private static Task<BankResponse> PaymentFailed(Payment payment, BankResponse bankResponse)
        {
            bankResponse.Status = PaymentProcessingStatus.Rejected;
            payment.Rejected();
            return Task.FromResult(bankResponse);
        }
    }
}