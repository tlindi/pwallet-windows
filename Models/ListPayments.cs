using System.Text.Json.Serialization;

namespace pWallet.Models;

public class IncomingPayment
{
	[JsonPropertyName("paymentHash")]
	public string PaymentHash { get; set; } = string.Empty;

	[JsonPropertyName("preimage")]
	public string? Preimage { get; set; }

	[JsonPropertyName("externalId")]
	public string? ExternalId { get; set; }

	[JsonPropertyName("description")]
	public string? Description { get; set; }

	[JsonPropertyName("invoice")]
	public string? Invoice { get; set; }

	[JsonPropertyName("isPaid")]
	public bool IsPaid { get; set; }

	[JsonPropertyName("receivedSat")]
	public long ReceivedSat { get; set; }

	[JsonPropertyName("fees")]
	public long Fees { get; set; }

	[JsonPropertyName("completedAt")]
	public long? CompletedAt { get; set; }

	[JsonPropertyName("createdAt")]
	public long CreatedAt { get; set; }
}

public class OutgoingPayment
{
	[JsonPropertyName("paymentId")]
	public string PaymentId { get; set; } = string.Empty;

	[JsonPropertyName("paymentHash")]
	public string PaymentHash { get; set; } = string.Empty;

	[JsonPropertyName("preimage")]
	public string? Preimage { get; set; }

	[JsonPropertyName("isPaid")]
	public bool IsPaid { get; set; }

	[JsonPropertyName("sent")]
	public long Sent { get; set; }

	[JsonPropertyName("fees")]
	public long Fees { get; set; }

	[JsonPropertyName("invoice")]
	public string? Invoice { get; set; }

	[JsonPropertyName("completedAt")]
	public long CompletedAt { get; set; }

	[JsonPropertyName("createdAt")]
	public long CreatedAt { get; set; }
}

public class PaymentEntry
{
	public DateTime Date { get; set; }
	public string Type { get; set; } = string.Empty;
	public long Amount { get; set; }
	public long Fee { get; set; }
}