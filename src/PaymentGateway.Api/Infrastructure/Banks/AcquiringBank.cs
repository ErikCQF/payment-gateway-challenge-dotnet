using System.Net;

using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Infrastructure.Banks
{
    public class AcquiringBank : IAcquiringBank
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AcquiringBank> _logger;

        public AcquiringBank(HttpClient httpClient, ILogger<AcquiringBank> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<BankResponse> ProcessPaymentAsync(BankRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/payments", request, cancellationToken);

                response.EnsureSuccessStatusCode();

                var bankResponse = await response.Content.ReadFromJsonAsync<BankResponse>(cancellationToken: cancellationToken);

                return bankResponse ?? throw new Exception("No Response from AcquiringBank API");

            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Bank simulator request failed with status {Status}", ex.StatusCode);
                throw new BankUnavailableException(ex.StatusCode ?? HttpStatusCode.ServiceUnavailable, ex);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Bank simulator request failed ");
                throw;
            }
        }
    }
}
