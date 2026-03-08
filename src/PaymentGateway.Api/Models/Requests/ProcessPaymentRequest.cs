namespace PaymentGateway.Api.Models.Requests
{
    public record ProcessPaymentRequest(string? CardNumber,
                                        int ExpiryMonth,
                                        int ExpiryYear,
                                        string? Currency,
                                        int Amount,
                                        string? Cvv);
}
