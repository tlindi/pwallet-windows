using System;
using Microsoft.JSInterop;

namespace pWallet.Services
{
	public class ConfigurationService
	{
		private readonly IJSRuntime _jsRuntime;

		public ConfigurationService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public async Task<string?> GetApiUrlAsync()
		{
			return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "apiUrl");
		}

		public async Task<string?> GetApiPasswordAsync()
		{
			return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "apiPassword");
		}

		public async Task<string> GetNormalizedApiUrlAsync()
		{
			var apiUrl = await GetApiUrlAsync();
			if (string.IsNullOrWhiteSpace(apiUrl)) return string.Empty;

			return NormalizeUrl(apiUrl);
		}
		public async Task<string?> GetWebSocketUrlAsync()
		{
			var apiUrl = await GetApiUrlAsync();
			if (string.IsNullOrWhiteSpace(apiUrl)) return null;

			// Ensure no trailing slash on the base URL
			apiUrl = apiUrl.TrimEnd('/');

			// Replace http/https with ws/wss
			var uriBuilder = new UriBuilder(apiUrl)
			{
				Scheme = apiUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase) ? "wss" : "ws"
			};

			// Append "/websocket"
			return $"{uriBuilder.Uri}websocket";
		}

		public static string NormalizeUrl(string url)
		{
			if (string.IsNullOrWhiteSpace(url)) return string.Empty;

			return url
				.Replace("http://", "", StringComparison.OrdinalIgnoreCase)
				.Replace("https://", "", StringComparison.OrdinalIgnoreCase)
				.Replace("ws://", "", StringComparison.OrdinalIgnoreCase)
				.Replace("wss://", "", StringComparison.OrdinalIgnoreCase)
				.TrimEnd('/');
		}
	}
}