using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Infrastructure.Validators.Rules
{
    public sealed class ValidateCurrency : IValidateRule
    {
        private static readonly HashSet<string> _supportedCurrencies = new(StringComparer.OrdinalIgnoreCase)
        {
            "GBP", "USD", "EUR"
        };

        public ProcessPaymentResponse Validate(ProcessPaymentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Currency))
            {
                return new ProcessPaymentResponse(false, "Currency is required.");
            }

            if (!_supportedCurrencies.Contains(request.Currency))
            {
                return new ProcessPaymentResponse(false, "Currency must be one of: GBP, USD, EUR.");
            }

            return new ProcessPaymentResponse(true, string.Empty);
        }
    }
}
