using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Infrastructure.Validators.Rules
{
    public sealed class ValidateCardNumber : IValidateRule
    {
        public ProcessPaymentResponse Validate(ProcessPaymentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.CardNumber))
            {
                return new ProcessPaymentResponse(false, "Card number is required.");
            }

            if (request.CardNumber.Length < 14 || request.CardNumber.Length > 19)
            {
                return new ProcessPaymentResponse(false, "Card number must be between 14 and 19 digits.");
            }

            if (!request.CardNumber.All(char.IsDigit))
            {
                return new ProcessPaymentResponse(false, "Card number must contain only numeric characters.");
            }

            return new ProcessPaymentResponse(true, nameof(ProcessPaymentResponse));

        }
    }

}
