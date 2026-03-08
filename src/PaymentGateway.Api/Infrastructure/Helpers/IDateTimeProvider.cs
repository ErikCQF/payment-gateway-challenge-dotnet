namespace PaymentGateway.Api.Infrastructure.Helpers
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
