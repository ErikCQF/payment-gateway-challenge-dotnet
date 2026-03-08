using System.Text.Json.Serialization;

namespace PaymentGateway.Api.Models.Requests
{
    public class BankRequest
    {
        [JsonPropertyName("card_number")]
        public string CardNumber { get; }

        [JsonPropertyName("expiry_date")]
        public string ExpiryDate { get; }

        [JsonPropertyName("currency")]
        public string Currency { get; }

        [JsonPropertyName("amount")]
        public int Amount { get; }

        [JsonPropertyName("cvv")]
        public string Cvv { get; }

        public BankRequest(string cardNumber, string expiryDate, string currency, int amount, string cvv)
        {
            CardNumber = cardNumber;
            ExpiryDate = expiryDate;
            Currency = currency;
            Amount = amount;
            Cvv = cvv;
        }
    }
}
