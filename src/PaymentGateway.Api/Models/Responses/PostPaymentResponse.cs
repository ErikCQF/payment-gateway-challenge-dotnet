using System.Text.Json.Serialization;

namespace PaymentGateway.Api.Models.Responses;

public class PostPaymentResponse
{
    //TODO: Nullable or non nullable. Can Not be nullable for best, but sending guid empty to clients sounds an id already was created
    public Guid? Id { get; set; }

    //spec says status returned need be Rejected/Authorised/Declined
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PaymentStatus Status { get; set; }
    public int CardNumberLastFour { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string Currency { get; set; }
    public int Amount { get; set; }
}
