using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Infrastructure.Validators.Rules
{
    public sealed class ValidateCvv : IValidateRule
    {
        public ProcessPaymentResponse Validate(ProcessPaymentRequest request)
        {

            if (string.IsNullOrWhiteSpace(request.Cvv))
            {
                return new ProcessPaymentResponse(false, "CVV is required.");
            }

            if (request.Cvv.Length < 3 || request.Cvv.Length > 4)
            {
                return new ProcessPaymentResponse(false, "CVV must be 3 or 4 characters.");
            }

            if (!request.Cvv.All(char.IsDigit))
            {
                return new ProcessPaymentResponse(false, "CVV must be only numbers.");
            }

            return new ProcessPaymentResponse(true, nameof(ProcessPaymentResponse));
        }
    }

}
