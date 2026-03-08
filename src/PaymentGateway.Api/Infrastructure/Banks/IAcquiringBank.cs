using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Infrastructure.Banks
{
    public interface IAcquiringBank
    {
        Task<BankResponse> ProcessPaymentAsync(BankRequest request, CancellationToken cancellationToken = default);
    }
}
