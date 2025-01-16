using System.Text.Json.Serialization;

namespace pWallet.Models;

public class PayBolt12OfferRequest
{
	[JsonPropertyName("amountSat")]
	public long? AmountSat { get; set; }

	[JsonPropertyName("offer")]
	public string Offer { get; set; } = string.Empty;

	[JsonPropertyName("message")]
	public string? Message { get; set; }
}

public class PayBolt12OfferResponse
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