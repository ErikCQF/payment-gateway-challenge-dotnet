using PaymentGateway.Api.Models.Responses;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Infrastructure.Repositories
{
    public class PaymentsRepositoryProxy : IPaymentsRepository
    {
        private readonly PaymentsRepository _paymentsRepository;

        public PaymentsRepositoryProxy(PaymentsRepository paymentsRepository)
        {
            _paymentsRepository = paymentsRepository;
        }

        public PostPaymentResponse? GetById(Guid id)
        {
            return _paymentsRepository.Get(id);
        }

        public void Add(PostPaymentResponse request, CancellationToken cancellationToken = default)
        {
            _paymentsRepository.Add(request);
        }
    }
}
