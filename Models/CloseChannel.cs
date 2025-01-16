using System.Text.Json.Serialization;

namespace pWallet.Models;

public class CloseChannelRequest
{
	[JsonPropertyName("channelId")]
	public string ChannelId { get; set; } = string.Empty;

	[JsonPropertyName("address")]
	public string Address { get; set; } = string.Empty;

	[JsonPropertyName("feerateSatByte")]
	public long FeerateSatByte { get; set; }
}

public class CloseChannelResponse
{
	[JsonPropertyName("txId")]
	public string TxId { get; set; } = string.Empty;
}