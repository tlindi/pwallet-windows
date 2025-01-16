using System.Text.Json.Serialization;

namespace pWallet.Models;

public class PaymentResponse
{
	[JsonPropertyName("recipientAmountSat")]
	public long RecipientAmountSat { get; set; }

	[JsonPropertyName("routingFeeSat")]
	public long RoutingFeeSat { get; set; }

	[JsonPropertyName("paymentId")]
	public string PaymentId { get; set; } = string.Empty;

	[JsonPropertyName("paymentHash")]
	public string PaymentHash { get; set; } = string.Empty;

	[JsonPropertyName("paymentPreimage")]
	public string PaymentPreimage { get; set; } = string.Empty;

	[JsonPropertyName("additionalInfo")]
	public string? AdditionalInfo { get; set; }

	public string? TxId {get;set;} = string.Empty;
}
