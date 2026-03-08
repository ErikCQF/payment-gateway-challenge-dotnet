using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Models
{
    public static class Extentions
    {
        public static ProcessPaymentRequest ToProcessPaymentRequest(this PostPaymentRequestModel request)
        {
            var expireDate = request?.ExpiryDate ?? "0/0";

            var parts = expireDate.Split('/');

            int.TryParse(parts.ElementAtOrDefault(0), out var month);
            int.TryParse(parts.ElementAtOrDefault(1), out var year);

            return new ProcessPaymentRequest(
                CardNumber: request?.CardNumber,
                ExpiryMonth: month,
                ExpiryYear: year,
                Currency: request?.Currency,
                Amount: request?.Amount ?? 0,
                Cvv: request?.Cvv);
        }

        public static BankResponse ToPostPaymentResponse(this PostPaymentResponse request)
        {
            return new BankResponse(request.Status == PaymentStatus.Authorized, request.Id.ToString());
        }
    }
}
