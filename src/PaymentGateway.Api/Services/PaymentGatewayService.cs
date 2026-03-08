using PaymentGateway.Api.Infrastructure.Banks;
using PaymentGateway.Api.Infrastructure.Repositories;
using PaymentGateway.Api.Infrastructure.Validators;
using PaymentGateway.Api.Models;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Services
{
    public class PaymentGatewayService : IPaymentGateway
    {
        private readonly ILogger<PaymentGatewayService> _logger;
        private readonly IValidatorService _validatorService;
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly IAcquiringBank _acquiringBank;

        public PaymentGatewayService(ILogger<PaymentGatewayService> logger,
                                    IValidatorService validatorService,
                                    IPaymentsRepository paymentsRepository,
                                    IAcquiringBank acquiringBank)
        {
            _logger = logger;
            _validatorService = validatorService;
            _paymentsRepository = paymentsRepository;
            _acquiringBank = acquiringBank;
        }
        public async Task<PostPaymentResponse> ProcessAsync(
                                                ProcessPaymentRequest request,
                                                CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            var lastFourChars = request.CardNumber![^4..];
            int.TryParse(lastFourChars, out var lastFourInt);
            var paymentId = Guid.NewGuid();

            var validation = _validatorService.Validate(request);

            if (!validation.IsValid)
            {
                var invaliResponse = BuildResponse(paymentId, request, lastFourInt, PaymentStatus.Rejected);
                _paymentsRepository.Add(invaliResponse);

                return invaliResponse;
            }
            BankRequest bankRequest = BuildBankRequest(request);

            var bankResponse = await _acquiringBank.ProcessPaymentAsync(bankRequest, cancellationToken);

            var status = bankResponse?.Authorized == true
                ? PaymentStatus.Authorized
                : PaymentStatus.Declined;

            var response = BuildResponse(paymentId, request, lastFourInt, status);

            _paymentsRepository.Add(response);

            return response;
        }

        private static BankRequest BuildBankRequest(ProcessPaymentRequest request)
        {
            return new BankRequest(
                cardNumber: request.CardNumber!,
                expiryDate: $"{request.ExpiryMonth:D2}/{request.ExpiryYear}",
                currency: request.Currency!,
                amount: request.Amount,
                cvv: request.Cvv!);
        }

        private static PostPaymentResponse BuildResponse(
            Guid id,
            ProcessPaymentRequest request,
            int lastFour,
            PaymentStatus status) => new()
            {
                Id = id,
                Status = status,
                CardNumberLastFour = lastFour,
                ExpiryMonth = request.ExpiryMonth,
                ExpiryYear = request.ExpiryYear,
                Currency = request.Currency!,
                Amount = request.Amount
            };
    }
}
