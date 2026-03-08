using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Services
{
    public interface IPaymentGateway
    {
        Task<PostPaymentResponse> ProcessAsync(ProcessPaymentRequest request, CancellationToken cancellationToken = default);
    }
}
