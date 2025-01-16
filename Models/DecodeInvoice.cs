using System.Text.Json.Serialization;

namespace pWallet.Models;

public class DecodeInvoiceRequest
{
	[JsonPropertyName("invoice")]
	public string Invoice { get; set; } = string.Empty;
}

public class ExtraHop
{
	[JsonPropertyName("nodeId")]
	public string NodeId { get; set; } = string.Empty;

	[JsonPropertyName("shortChannelId")]
	public string ShortChannelId { get; set; } = string.Empty;

	[JsonPropertyName("feeBase")]
	public int FeeBase { get; set; }

	[JsonPropertyName("feeProportionalMillionths")]
	public int FeeProportionalMillionths { get; set; }

	[JsonPropertyName("cltvExpiryDelta")]
	public int CltvExpiryDelta { get; set; }
}

public class DecodeInvoiceResponse
{
	[JsonPropertyName("chain")]
	public string Chain { get; set; } = string.Empty;

	[JsonPropertyName("amount")]
	public long? Amount { get; set; }

	[JsonPropertyName("paymentHash")]
	public string PaymentHash { get; set; } = string.Empty;

	[JsonPropertyName("description")]
	public string Description { get; set; } = string.Empty;

	[JsonPropertyName("minFinalCltvExpiryDelta")]
	public int MinFinalCltvExpiryDelta { get; set; }

	[JsonPropertyName("paymentSecret")]
	public string PaymentSecret { get; set; } = string.Empty;

	[JsonPropertyName("paymentMetadata")]
	public string? PaymentMetadata { get; set; }

	[JsonPropertyName("extraHops")]
	public List<List<ExtraHop>> ExtraHops { get; set; } = new();

	[JsonPropertyName("features")]
	public Features Features { get; set; } = new();

	[JsonPropertyName("timestampSeconds")]
	public long TimestampSeconds { get; set; }

	public string? OnChainAddress { get; set; } = string.Empty;
	public int? FeeRateOnChain { get; set; } = null;
	public string? TxId { get; set; } = string.Empty;

	public string? Address { get; set; } = string.Empty;
}

public class Features
{
	[JsonPropertyName("activated")]
	public Dictionary<string, string> Activated { get; set; } = new();

	[JsonPropertyName("unknown")]
	public List<int> Unknown { get; set; } = new(); // Change List<string> to List<int>
}
