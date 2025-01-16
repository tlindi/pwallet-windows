using System.Text.Json.Serialization;

namespace pWallet.Models;

public class PayBolt11Invoice
{
	[JsonPropertyName("amountSat")]
	public long? AmountSat { get; set; }

	[JsonPropertyName("invoice")]
	public string Invoice { get; set; } = string.Empty;
}

public class PayBolt11InvoiceResponse
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