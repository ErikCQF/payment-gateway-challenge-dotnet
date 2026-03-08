using Moq;
using PaymentGateway.Api.Services;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Infrastructure.Validators.Rules;
using PaymentGateway.Api.Models.Responses;
using PaymentGateway.Api.Infrastructure.Validators;

namespace PaymentGateway.Api.Tests
{
    public class ValidatorServiceTests
    {
        private readonly ProcessPaymentRequest _request = new(
                         CardNumber: default,
                         ExpiryMonth: default,
                         ExpiryYear: default,
                         Currency: default,
                         Amount: default,
                         Cvv: default);

        private static Mock<IValidateRule> RuleThatPasses()
        {
            var mock = new Mock<IValidateRule>();
            mock.Setup(r => r.Validate(It.IsAny<ProcessPaymentRequest>()))
                .Returns(new ProcessPaymentResponse(true, string.Empty));
            return mock;
        }

        private static Mock<IValidateRule> RuleThatFails(string message)
        {
            var mock = new Mock<IValidateRule>();
            mock.Setup(r => r.Validate(It.IsAny<ProcessPaymentRequest>()))
                .Returns(new ProcessPaymentResponse(false, message));
            return mock;
        }

        [Fact]
        public void Validation_ok()
        {
            var rule1 = RuleThatPasses();
            var rule2 = RuleThatPasses();
            var validator = new ValidatorService(new[] { rule1.Object, rule2.Object });

            var result = validator.Validate(_request);

            rule1.Verify(r => r.Validate(_request), Times.Once);
            rule2.Verify(r => r.Validate(_request), Times.Once);

            Assert.True(result.IsValid);

        }
        [Fact]
        public void Validation_not_ok()
        {
            var errorMessage = "FAIL";
            var rule1 = RuleThatFails(errorMessage);
            var rule2 = RuleThatPasses();
            var validator = new ValidatorService(new[] { rule1.Object, rule2.Object });

            var result = validator.Validate(_request);

            rule1.Verify(r => r.Validate(_request), Times.Once);
            rule2.Verify(r => r.Validate(_request), Times.Once);

            Assert.False(result.IsValid);
            Assert.Equal("FAIL", result?.Message);

        }
    }
}
