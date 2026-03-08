using System.Net;

namespace PaymentGateway.Api.Infrastructure.Banks
{
    public class BankUnavailableException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public BankUnavailableException(HttpStatusCode statusCode, Exception? innerException = null)
            : base($"Acquiring bank is unavailable. Status: {statusCode}", innerException)
        {
            StatusCode = statusCode;
        }
    }
}
