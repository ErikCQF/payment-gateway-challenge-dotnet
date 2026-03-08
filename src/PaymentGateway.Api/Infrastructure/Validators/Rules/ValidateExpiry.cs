using PaymentGateway.Api.Infrastructure.Helpers;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Infrastructure.Validators.Rules
{
    public sealed class ValidateExpiry : IValidateRule
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public ValidateExpiry(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public ProcessPaymentResponse Validate(ProcessPaymentRequest request)
        {
            if (request.ExpiryMonth < 1 || request.ExpiryMonth > 12)
            {
                return new ProcessPaymentResponse(false, "Expiry month must be between 1 and 12.");
            }

            var now = _dateTimeProvider.UtcNow;

            if (request.ExpiryYear < now.Year ||
                request.ExpiryYear == now.Year && request.ExpiryMonth < now.Month)
            {
                return new ProcessPaymentResponse(false, "Card expiry date must be in the future.");
            }

            return new ProcessPaymentResponse(true, nameof(ValidateExpiry));

        }
    }

}
