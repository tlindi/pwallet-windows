using System.Text.Json.Serialization;

namespace pWallet.Models;

public class NodeInfoResponse
{
	[JsonPropertyName("nodeId")]
	public string NodeId { get; set; } = string.Empty;

	[JsonPropertyName("channels")]
	public List<ChannelInfo> Channels { get; set; } = new();

	public class ChannelInfo
	{
		[JsonPropertyName("state")]
		public string State { get; set; } = string.Empty;

		[JsonPropertyName("channelId")]
		public string ChannelId { get; set; } = string.Empty;

		[JsonPropertyName("balanceSat")]
		public long BalanceSat { get; set; }

		[JsonPropertyName("inboundLiquiditySat")]
		public long InboundLiquiditySat { get; set; }

		[JsonPropertyName("capacitySat")]
		public long CapacitySat { get; set; }

		[JsonPropertyName("fundingTxId")]
		public string FundingTxId { get; set; } = string.Empty;
	}
}