using System.Text.Json.Serialization;

namespace pWallet.Models;

public class PayLightningAddressRequest
{
	[JsonPropertyName("amountSat")]
	public long? AmountSat { get; set; }

	[JsonPropertyName("address")]
	public string Address { get; set; } = string.Empty;

	[JsonPropertyName("message")]
	public string? Message { get; set; }
}

public class PayLightningAddressResponse
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
