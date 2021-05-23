using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.PaymentGateway.ExternalClients;
using Checkout.PaymentGateway.Models.Core.Entities;
using Checkout.PaymentGateway.Models.Dtos;
using Checkout.PaymentGateway.Repositories.Repositories;

namespace Checkout.PaymentGateway.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBankingProxy _bankingProxy;
        private readonly IMapper _mapper;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IBankingProxy bankingProxy,
            IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _bankingProxy = bankingProxy;
            _mapper = mapper;
        }

        public async Task<PaymentResponse> Pay(PaymentRequest request)
        {
            var payment = _mapper.Map<Payment>(request);
            var validationResult = await payment.Validate();
            return validationResult.IsValid switch
            {
                true => await ProcessPayment(payment),
                _ => new PaymentResponseFailed(validationResult.Errors.Select(e => new PaymentErrors(e.ErrorMessage))
                    .ToArray()
                    .ToList())
            };
        }

        public async Task<PaymentResponse> Get(Guid id)
        {
            var payment = await _paymentRepository.GetByIdIncludingBankCard(id);
            return payment switch
            {
                null => new PaymentResponseFailed(new[] {new PaymentErrors($"Payment not found for id {id}")}.ToList()),
                _ => new PaymentResponseSuccess(id, _mapper.Map<PaymentRequest>(payment), payment.ProcessingStatus)
            };
        }

        private async Task<PaymentResponse> ProcessPayment(Payment payment)
        {
            await _bankingProxy.ProcessPaymentRequest(payment);
            await _paymentRepository.Save(payment);
            return new PaymentResponseSuccess(payment.Id, _mapper.Map<PaymentRequest>(payment),
                payment.ProcessingStatus);
        }
    }
}