using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace pWallet.Services
{
    public interface IBitcoinPriceService
    {
        Task<double?> GetBitcoinPriceInUsdAsync();
    }

    public class BitcoinPriceService : IBitcoinPriceService, IHostedService, IDisposable
    {
        private readonly HttpClient _http;
        private readonly ILogger<BitcoinPriceService> _logger;
        private Timer? _timer;
        private double? _cachedUsdPrice;
        private DateTime _lastUpdateTime = DateTime.MinValue;
        private readonly TimeSpan _updateInterval = TimeSpan.FromMinutes(16);

        public BitcoinPriceService(HttpClient http, ILogger<BitcoinPriceService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Bitcoin Price Service started.");
            _timer = new Timer(UpdatePrice, null, TimeSpan.Zero, _updateInterval);
            return Task.CompletedTask;
        }

        public async Task<double?> GetBitcoinPriceInUsdAsync()
        {
            if (DateTime.Now - _lastUpdateTime < _updateInterval)
            {
                return _cachedUsdPrice;
            }

            await UpdatePriceAsync();
            return _cachedUsdPrice;
        }

        private async Task UpdatePriceAsync()
        {
	        try
	        {
		        var response = await _http.GetAsync("https://blockchain.info/ticker");

		        if (!response.IsSuccessStatusCode)
		        {
			        _logger.LogWarning("Failed to fetch Bitcoin price. Status: {StatusCode}", response.StatusCode);
			        return;
		        }

		        var jsonResponse = await response.Content.ReadAsStringAsync();
		        var tickerData = JsonSerializer.Deserialize<Dictionary<string, CurrencyTicker>>(jsonResponse);

		        if (tickerData != null && tickerData.TryGetValue("USD", out var usdTicker))
		        {
			        _cachedUsdPrice = usdTicker.Last;
			        _lastUpdateTime = DateTime.Now;
			        _logger.LogInformation("Bitcoin price updated to {Price} USD.", _cachedUsdPrice);
		        }
		        else
		        {
			        _logger.LogWarning("USD data not found in the response.");
		        }
	        }
	        catch (Exception ex)
	        {
		        _logger.LogError(ex, "Error fetching Bitcoin price.");
	        }
        }

        private void UpdatePrice(object? state)
        {
            _ = UpdatePriceAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Bitcoin Price Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

    public class CurrencyTicker
    {
	    [JsonPropertyName("last")]
	    public double Last { get; set; }  // Represents the "last" field in the JSON
	    [JsonPropertyName("symbol")]
	    public string Symbol { get; set; } = string.Empty;  // Represents the "symbol" field
    }
}
