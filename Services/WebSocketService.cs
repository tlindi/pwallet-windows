using System.Text;
using Websocket.Client;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using pWallet.Controllers;

namespace pWallet.Services
{
    public class WebSocketService : IDisposable
    {
        private readonly ConfigurationService _configurationService;
        private WebsocketClient? _webSocketClient;
        private readonly ConcurrentDictionary<string, List<Action<string>>> _messageHandlers = new();

        private readonly ConcurrentDictionary<string, ConcurrentBag<SseController.SseClient>> _sseClients;

        public WebSocketService(
	        ConfigurationService configurationService,
	        ConcurrentDictionary<string, ConcurrentBag<SseController.SseClient>> sseClients)
        {
	        _configurationService = configurationService;
	        _sseClients = sseClients;
        }

        public async Task StartWebSocketAsync()
        {
            var webSocketUrl = await _configurationService.GetWebSocketUrlAsync();
            var apiPassword = await _configurationService.GetApiPasswordAsync();

            if (string.IsNullOrWhiteSpace(webSocketUrl) || string.IsNullOrWhiteSpace(apiPassword))
            {
                throw new InvalidOperationException("WebSocket URL or API password is missing.");
            }

            var factory = new Func<ClientWebSocket>(() =>
            {
                var client = new ClientWebSocket();
                var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(":" + apiPassword));
                client.Options.SetRequestHeader("Authorization", $"Basic {token}");
                return client;
            });

            _webSocketClient = new WebsocketClient(new Uri(webSocketUrl), factory)
            {
                ReconnectTimeout = TimeSpan.FromSeconds(30)
            };

            _webSocketClient.ReconnectionHappened.Subscribe(info =>
            {
            });

            _webSocketClient.MessageReceived.Subscribe(msg =>
            {
                if (msg.Text != null) HandleMessage(msg.Text, NormalizeUrl(webSocketUrl));
            });

            await _webSocketClient.Start();
        }

        public void AddMessageHandler(string key, Action<string> handler)
        {
            key = NormalizeUrl(key);
            if (!_messageHandlers.ContainsKey(key))
            {
                _messageHandlers[key] = new List<Action<string>>();
            }
            _messageHandlers[key].Add(handler);
        }

        private void HandleMessage(string message, string key)
        {
	        if (_sseClients.TryGetValue(key, out var clients))
	        {
		        foreach (var client in clients)
		        {
			        client.SendAsync(message).Wait();
		        }
	        }

	        if (_messageHandlers.TryGetValue(key, out var handlers))
	        {
		        foreach (var handler in handlers)
		        {
			        handler.Invoke(message);
		        }
	        }
        }

        public static string NormalizeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return string.Empty;

            var normalizedUrl = url
                .Replace("http://", "", StringComparison.OrdinalIgnoreCase)
                .Replace("https://", "", StringComparison.OrdinalIgnoreCase)
                .Replace("ws://", "", StringComparison.OrdinalIgnoreCase)
                .Replace("wss://", "", StringComparison.OrdinalIgnoreCase)
                .Replace("/websocket", "", StringComparison.OrdinalIgnoreCase)
                .TrimEnd('/');

            return normalizedUrl;
        }

        public void Dispose()
        {
            _webSocketClient?.Dispose();
        }
    }
}
