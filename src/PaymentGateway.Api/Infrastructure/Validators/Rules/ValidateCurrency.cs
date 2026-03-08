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
            if (_supportedCurrencies.Contains(request.Currency!))
            {
                return new ProcessPaymentResponse(true, string.Empty);
            }

            return new ProcessPaymentResponse(false, nameof(ValidateCurrency));
        }
    }
}
