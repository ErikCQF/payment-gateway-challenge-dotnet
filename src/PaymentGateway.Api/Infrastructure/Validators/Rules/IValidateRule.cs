using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Infrastructure.Validators.Rules
{
    public interface IValidateRule
    {
        ProcessPaymentResponse Validate(ProcessPaymentRequest request);
    }
}
