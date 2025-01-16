using System.Text.Json.Serialization;

namespace pWallet.Models;

public class GetBolt12OfferResponse
{
	[JsonPropertyName("offer")]
	public string Offer { get; set; } = string.Empty;
}

public class GetLightningAddressResponse
{
	[JsonPropertyName("lightningAddress")]
	public string LightningAddress { get; set; } = string.Empty;
}
