using System.Text.Json.Serialization;

namespace PaymentGateway.Api.Models.Responses
{
    public class BankResponse
    {
        [JsonPropertyName("authorized")]
        public bool Authorized { get; }

        [JsonPropertyName("authorization_code")]
        public string? AuthorizationCode { get; }

        public BankResponse(bool authorized, string? authorizationCode)
        {
            Authorized = authorized;
            AuthorizationCode = authorizationCode;
        }
    }
}
