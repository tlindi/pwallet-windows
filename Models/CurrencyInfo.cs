using System.Text.Json.Serialization;

namespace pWallet.Models;

public class CurrencyInfo
{
    [JsonPropertyName("last")]
    public double Last { get; set; }

    [JsonPropertyName("highest")]
    public double Highest { get; set; }

    [JsonPropertyName("lowest")]
    public double Lowest { get; set; }
}