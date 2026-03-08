using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Infrastructure.Repositories
{
    public interface IPaymentsRepository
    {
        void Add(PostPaymentResponse request, CancellationToken cancellationToken = default);
        PostPaymentResponse? GetById(Guid id);
    }
}
