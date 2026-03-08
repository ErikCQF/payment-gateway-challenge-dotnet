using PaymentGateway.Api.Infrastructure.Validators.Rules;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Infrastructure.Validators
{
    public class ValidatorService : IValidatorService
    {
        private readonly IEnumerable<IValidateRule> _validationRules;

        public ValidatorService(IEnumerable<IValidateRule> validationRules)
        {
            _validationRules = validationRules ?? throw new ArgumentNullException(nameof(validationRules));
        }
        public ProcessPaymentResponse Validate(ProcessPaymentRequest request)
        {
            var validations = _validationRules
                 .Select(x => x.Validate(request))
                 .Where(r => !r.IsValid)
                 .Select(r => r.Message)
                 .ToList();

            if (validations.Count == 0)
            {
                return new ProcessPaymentResponse(true, nameof(ValidatorService));
            }

            return new ProcessPaymentResponse(false, string.Join(", ", validations));
        }
    }
}
