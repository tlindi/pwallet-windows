using System.Text.Json.Serialization;

namespace pWallet.Models;

public class PayOnChainRequest
{
	[JsonPropertyName("amountSat")]
	public long? AmountSat { get; set; }

	[JsonPropertyName("address")]
	public string? Address { get; set; } = string.Empty;

	[JsonPropertyName("feerateSatByte")]
	public int? FeerateSatByte { get; set; }
}

public class PayOnChainResponse
{
	[JsonPropertyName("transactionId")]
	public string TransactionId { get; set; } = string.Empty;
}

