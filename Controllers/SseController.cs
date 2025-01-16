using Microsoft.AspNetCore.Mvc;
using pWallet.Services;
using System.Collections.Concurrent;
using System.Text;

namespace pWallet.Controllers
{
    [ApiController]
    [Route("api/sse")]
    public class SseController : ControllerBase
    {
        private readonly WebSocketService _webSocketService;

        private readonly ConcurrentDictionary<string, ConcurrentBag<SseClient>> _clients;

        public SseController(
	        WebSocketService webSocketService,
	        ConcurrentDictionary<string, ConcurrentBag<SseClient>> clients)
        {
	        _webSocketService = webSocketService;
	        _clients = clients;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Connect(string key)
        {
	        var normalizedKey = NormalizeUrl(key);

	        Response.Headers.Add("Content-Type", "text/event-stream");
	        Response.Headers.Add("Cache-Control", "no-cache");
	        Response.Headers.Add("Connection", "keep-alive");

	        if (!_clients.ContainsKey(normalizedKey))
	        {
		        _clients[normalizedKey] = new ConcurrentBag<SseClient>();
	        }

	        var client = new SseClient(Response.Body);
	        _clients[normalizedKey].Add(client);

	        try
	        {
		        await client.KeepAliveAsync();
	        }
	        catch (Exception ex)
	        {
		        Console.WriteLine($"SSE client disconnected: {ex.Message}");
	        }
	        finally
	        {
		        _clients[normalizedKey].TryTake(out _);
	        }

	        return new EmptyResult();
        }

        private static string NormalizeUrl(string url)
        {
            return url.Replace("http://", "").Replace("https://", "").TrimEnd('/');
        }

        public class SseClient
        {
	        private readonly Stream _responseStream;
	        private static readonly byte[] Heartbeat = Encoding.UTF8.GetBytes(":\n\n");

	        public SseClient(Stream responseStream)
	        {
		        _responseStream = responseStream;
	        }

	        public async Task SendAsync(string message)
	        {
		        try
		        {
			        var data = string.Join("\n", message.Split('\n').Select(line => $"data: {line}")) + "\n\n";
			        var bytes = Encoding.UTF8.GetBytes(data);

			        await _responseStream.WriteAsync(bytes, 0, bytes.Length);
			        await _responseStream.FlushAsync();
		        }
		        catch (Exception ex)
		        {
			        Console.WriteLine($"Error sending SSE message: {ex.Message}");
		        }
	        }


	        public async Task KeepAliveAsync()
	        {
		        while (true)
		        {
			        try
			        {
				        await _responseStream.WriteAsync(Heartbeat, 0, Heartbeat.Length);
				        await _responseStream.FlushAsync();
				        await Task.Delay(15000);
			        }
			        catch (Exception ex)
			        {
				        Console.WriteLine($"Error in KeepAlive: {ex.Message}");
				        break;
			        }
		        }
	        }
        }

    }
}
