using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Infrastructure.Validators.Rules
{
    public sealed class ValidateAmount : IValidateRule
    {
        public ProcessPaymentResponse Validate(ProcessPaymentRequest request)
        {
            if (request.Amount <= 0)
            {
                return new ProcessPaymentResponse(false, "Amount must be a positive value");
            }

            return new ProcessPaymentResponse(true, nameof(ValidateAmount));
        }
    }

}
