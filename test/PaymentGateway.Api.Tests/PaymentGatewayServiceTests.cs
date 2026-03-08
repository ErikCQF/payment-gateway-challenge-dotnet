using Microsoft.Extensions.Logging;

using Moq;

using PaymentGateway.Api.Infrastructure.Repositories;
using PaymentGateway.Api.Infrastructure.Validators;
using PaymentGateway.Api.Models;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Tests
{
    public class PaymentGatewayServiceTests
    {
        private readonly Mock<ILogger<PaymentGatewayService>> _loggerMock = new();
        private readonly Mock<IValidatorService> _validatorMock = new();
        private readonly Mock<IPaymentsRepository> _repositoryMock = new();
        private readonly Mock<IAcquiringBank> _bankMock = new();
        private readonly IPaymentGateway _paymentGateway;

        // Valid baseline request
        private static readonly ProcessPaymentRequest ValidRequest = new(
            CardNumber: "2222405343248877",
            ExpiryMonth: DateTime.UtcNow.Month,
            ExpiryYear: DateTime.UtcNow.Year + 1,
            Currency: "GBP",
            Amount: 1050,
            Cvv: "123");

        public PaymentGatewayServiceTests()
        {
            _paymentGateway = new PaymentGatewayService(
                _loggerMock.Object,
                _validatorMock.Object,
                _repositoryMock.Object,
                _bankMock.Object);
        }

        [Fact]
        public async Task Authorized_bank_response_returns_authorized_status()
        {
            //Arrange
            _validatorMock
             .Setup(v => v.Validate(It.IsAny<ProcessPaymentRequest>()))
             .Returns(new ProcessPaymentResponse(true, "Valid"));
            _bankMock
                .Setup(b => b.ProcessPaymentAsync(It.IsAny<BankRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BankResponse(true, "auth-123"));

            //Act
            var result = await _paymentGateway.ProcessAsync(ValidRequest);

            //Assert
            Assert.Equal(PaymentStatus.Authorized, result.Status);
        }
    }
}
