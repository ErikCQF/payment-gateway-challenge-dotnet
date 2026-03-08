namespace PaymentGateway.Api.Models.Responses
{
    public record ProcessPaymentResponse(bool IsValid,
                                        string Message);
}
