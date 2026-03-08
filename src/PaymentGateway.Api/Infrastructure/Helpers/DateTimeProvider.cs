namespace PaymentGateway.Api.Infrastructure.Helpers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
