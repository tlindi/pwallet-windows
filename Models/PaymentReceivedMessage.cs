using System.Text.Json.Serialization;

namespace pWallet.Models;
	public class PaymentReceivedMessage
	{
		[JsonPropertyName("type")]
		public string Type { get; set; } = string.Empty;

		[JsonPropertyName("amountSat")]
		public long AmountSat { get; set; }

		[JsonPropertyName("paymentHash")]
		public string PaymentHash { get; set; } = string.Empty;

		[JsonPropertyName("timestamp")]
		public long Timestamp { get; set; }

		[JsonPropertyName("externalId")]
		public string? ExternalId { get; set; }

		[JsonPropertyName("payerNote")]
		public string? PayerNote { get; set; }

		[JsonPropertyName("payerKey")]
		public string? PayerKey { get; set; }
	}