using System.Text.Json.Serialization;

namespace pWallet.Models;

public class LnUrlPayRequest
{
	[JsonPropertyName("amountSat")]
	public long? AmountSat { get; set; }

	[JsonPropertyName("lnurl")]
	public string LnUrl { get; set; } = string.Empty;

	[JsonPropertyName("message")]
	public string? Message { get; set; }
}

public class LnUrlPayResponse
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
}