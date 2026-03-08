using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Infrastructure.Validators
{
    public interface IValidatorService
    {
        ProcessPaymentResponse Validate(ProcessPaymentRequest request);
    }
}
