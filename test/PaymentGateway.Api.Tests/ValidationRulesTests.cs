using Moq;

using PaymentGateway.Api.Infrastructure.Helpers;
using PaymentGateway.Api.Infrastructure.Validators.Rules;
using PaymentGateway.Api.Models.Requests;

namespace PaymentGateway.Api.Tests
{
    public class ValidationRulesTests
    {

        [Theory]
        [InlineData("GBP", true)]
        [InlineData("USD", true)]
        [InlineData("EUR", true)]
        [InlineData("BRL", false)]
        public void Currency_is_valid(string currency, bool expectedResult)
        {
            //Arrage
            ValidateCurrency validator = new();
            var request = new ProcessPaymentRequest(
                CardNumber: default,
                ExpiryMonth: default,
                ExpiryYear: default,
                Currency: currency,
                Amount: default,
                Cvv: default);
            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.Equal(expectedResult, result.IsValid);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(10, true)]

        public void Amount_is_valid(int amount, bool expectedResult)
        {
            //Arrage
            ValidateAmount validator = new();
            var request = new ProcessPaymentRequest(
                CardNumber: default,
                ExpiryMonth: default,
                ExpiryYear: default,
                Currency: default,
                Amount: amount,
                Cvv: default);

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.Equal(expectedResult, result.IsValid);
        }

        [Theory]
        [InlineData(1, 2027, "2026-01-01", true)]
        [InlineData(6, 2026, "2026-06-01", true)]
        [InlineData(1, 2025, "2026-01-01", false)] // Expired
        [InlineData(5, 2026, "2026-06-01", false)] // Expired 
        public void Card_month_and_year_is_valid(int expiryMonth, int expiryYear, string now, bool expectedResult)
        {
            // Arrange
            var fakeNow = DateTime.SpecifyKind(DateTime.Parse(now), DateTimeKind.Utc);
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(p => p.UtcNow).Returns(fakeNow);
            var validator = new ValidateExpiry(dateTimeProviderMock.Object);

            var request = new ProcessPaymentRequest(
                CardNumber: default,
                ExpiryMonth: expiryMonth,
                ExpiryYear: expiryYear,
                Currency: default,
                Amount: default,
                Cvv: default);

            // Act
            var result = validator.Validate(request);

            // Assert
            Assert.Equal(expectedResult, result.IsValid);
        }

        [Theory]
        [InlineData("123", true)]
        [InlineData("1234", true)]
        [InlineData("12 4", false)]
        [InlineData("13d", false)]
        [InlineData("13m", false)]
        public void Valid_cvv_returns_valid(string cvv, bool expectedResult)
        {
            //Arrage
            ValidateCvv validator = new();
            var request = new ProcessPaymentRequest(
                CardNumber: default,
                ExpiryMonth: default,
                ExpiryYear: default,
                Currency: default,
                Amount: default,
                Cvv: cvv);
            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.Equal(expectedResult, result.IsValid);
        }


        [Theory]
        [InlineData("22224053432488", true)]       // 14 digits — minimum
        [InlineData("2222405343248877", true)]     // 16 digits — typical Visa
        [InlineData("1234567890123456789", true)]  // 19 digits — maximum
        [InlineData("12345678901234S56789", false)]
        [InlineData("123456789012345XXX6789", false)]
        [InlineData("22224X53432488", false)]

        public void Valid_card_number_returns_valid(string cardNumber, bool expectedResult)
        {
            ValidateCardNumber validator = new();
            var request = new ProcessPaymentRequest(
                CardNumber: cardNumber,
                ExpiryMonth: default,
                ExpiryYear: default,
                Currency: default,
                Amount: default,
                Cvv: default);

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.Equal(expectedResult, result.IsValid);
        }

    }
}
