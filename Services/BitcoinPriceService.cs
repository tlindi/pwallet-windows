using System.Text.Json;
using pWallet.Models;

namespace pWallet.Services
{
    public interface IBitcoinPriceService
    {
        event Func<Task> OnPriceUpdated;
        Task<BitcoinPriceResponse?> GetBitcoinPriceAsync();
        double OneUnitCurrencyInSats(double currency);
    }

    public class BitcoinPriceService : IBitcoinPriceService, IHostedService, IDisposable
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BitcoinPriceService> _logger;
        private Timer _timer;

        private BitcoinPriceResponse? _cachedPrice;
        private DateTime _lastUpdateDate = DateTime.MinValue;
        private readonly TimeSpan _updateInterval = TimeSpan.FromMinutes(15);

        public BitcoinPriceService(HttpClient http, IConfiguration configuration, ILogger<BitcoinPriceService> logger)
        {
            _http = http;
            _configuration = configuration;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Bitcoin Price Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(1.5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _ = UpdatePriceAsync();
        }

        public async Task<BitcoinPriceResponse?> GetBitcoinPriceAsync()
        {
            if (DateTime.Now - _lastUpdateDate < _updateInterval)
            {
                return _cachedPrice;
            }

            try
            {
                var apiKey = _configuration.GetValue<string>("ApiKey");
                var url = _configuration.GetValue<string>("ApiUrl");

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                requestMessage.Headers.Add("ApiKey", apiKey);

                var response = await _http.SendAsync(requestMessage);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error: {response.StatusCode} - {errorContent}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                _cachedPrice = JsonSerializer.Deserialize<BitcoinPriceResponse>(jsonResponse);
                _lastUpdateDate = DateTime.Now;

                _logger.LogInformation("Bitcoin Price updated at: {time}", DateTimeOffset.Now);
                return _cachedPrice;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Bitcoin price.");
                return null;
            }
        }

        
        public double OneUnitCurrencyInSats(double currency)
        {
            const int oneBitcoin = 100000000;
            return oneBitcoin / currency;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Bitcoin Price Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public event Func<Task> OnPriceUpdated;

        private async Task UpdatePriceAsync()
        {
            var now = DateTime.Now;
            if (now - _lastUpdateDate >= _updateInterval)
            {
                _cachedPrice = await GetBitcoinPriceAsync();
                _lastUpdateDate = now;

                if (_cachedPrice != null)
                {
                    OnPriceUpdated?.Invoke();
                }
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
