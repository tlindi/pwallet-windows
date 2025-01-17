using System.Text.Json.Serialization;

namespace pWallet.Models;

public class BitcoinPriceResponse
{
	[JsonPropertyName("eur")]
	public CurrencyInfo Eur { get; set; }

	[JsonPropertyName("sek")]
	public CurrencyInfo Sek { get; set; }

	[JsonPropertyName("usd")]
	public CurrencyInfo? Usd { get; set; }

	public DateTime LastUpdated { get; set; }

}