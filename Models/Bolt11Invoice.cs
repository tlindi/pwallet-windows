using System.Text.Json.Serialization;

namespace pWallet.Models;

public class Bolt11InvoiceRequest
{
	public string? Description { get; set; } = string.Empty;
	public long? AmountSat { get; set; }
	public int ExpirySeconds { get; set; } = 3600;
	public string? ExternalId { get; set; }
	public string? WebhookUrl { get; set; }
}

public class Bolt11InvoiceResponse
{
	[JsonPropertyName("amountSat")]
	public long AmountSat { get; set; }

	[JsonPropertyName("paymentHash")]
	public string PaymentHash { get; set; } = string.Empty;

	[JsonPropertyName("serialized")]
	public string Serialized { get; set; } = string.Empty;
}