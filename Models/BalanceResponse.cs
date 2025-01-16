using System.Text.Json.Serialization;

namespace pWallet.Models;

public class BalanceResponse
{
	[JsonPropertyName("balanceSat")]
	public long BalanceSat { get; set; }

	[JsonPropertyName("feeCreditSat")]
	public long FeeCreditSat { get; set; }
}