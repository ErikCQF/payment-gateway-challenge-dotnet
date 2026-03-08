using PaymentGateway.Api.Models.Responses;

using System.Collections.Concurrent;
using System.IO.Pipes;

namespace PaymentGateway.Api.Services;

public class PaymentsRepository
{
    public ConcurrentDictionary<Guid, PostPaymentResponse> Payments = new();

    public void Add(PostPaymentResponse payment)
    {
        if (payment.Id == null) throw new ArgumentNullException("missing id");

        Payments[payment.Id.Value] = payment;
    }
    public PostPaymentResponse? Get(Guid id)
    {
        if (Payments.TryGetValue(id, out var payment))
        {
            return payment;
        };

        return null;
    }
}