using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using pWallet.Enums;
using pWallet.Extensions;
using pWallet.Interfaces;
using pWallet.Models;

namespace pWallet.Services;

public class PhoenixService : INodeService, IPaymentService, ILnUrlService
{
	private readonly HttpClient _httpClient;
	private readonly ConfigurationService _configurationService;

	public PhoenixService(HttpClient httpClient, ConfigurationService configurationService)
	{
		_httpClient = httpClient;
		_configurationService = configurationService;
	}

	private HttpClient CreateHttpClient(string apiUrl, string apiPassword)
	{
		var client = new HttpClient { BaseAddress = new Uri(apiUrl) };
		var byteArray = Encoding.UTF8.GetBytes($":{apiPassword}");
		client.DefaultRequestHeaders.Authorization =
			new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
		return client;
	}

	public async Task<NodeInfoResponse?> GetNodeInfoAsync()
	{
		try
		{
			var apiUrl = await _configurationService.GetApiUrlAsync();
			var apiPassword = await _configurationService.GetApiPasswordAsync();

			if (string.IsNullOrWhiteSpace(apiUrl) || string.IsNullOrWhiteSpace(apiPassword))
			{
				throw new InvalidOperationException("API URL or API password is missing.");
			}

			using var client = CreateHttpClient(apiUrl, apiPassword);

			var response = await client.GetAsync("/getinfo");
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<NodeInfoResponse>(json);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error fetching node info: {ex.Message}");
			return null;
		}
	}

	public async Task<BalanceResponse?> GetBalanceAsync()
	{
		try
		{
			var apiUrl = await _configurationService.GetApiUrlAsync();
			var apiPassword = await _configurationService.GetApiPasswordAsync();

			if (string.IsNullOrWhiteSpace(apiUrl) || string.IsNullOrWhiteSpace(apiPassword))
			{
				throw new InvalidOperationException("API URL or API password is missing.");
			}

			using var client = CreateHttpClient(apiUrl, apiPassword);

			var response = await client.GetAsync("/getbalance");
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<BalanceResponse>(json);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error fetching balance: {ex.Message}");
			return null;
		}
	}
	
	public async Task<CloseChannelResponse?> CloseChannelAsync(CloseChannelRequest request)
	{
		try
		{
			var apiUrl = await _configurationService.GetApiUrlAsync();
			var apiPassword = await _configurationService.GetApiPasswordAsync();

			if (string.IsNullOrWhiteSpace(apiUrl) || string.IsNullOrWhiteSpace(apiPassword))
			{
				throw new InvalidOperationException("API URL or API password is missing.");
			}

			using var client = CreateHttpClient(apiUrl, apiPassword);

			var content = new FormUrlEncodedContent(new Dictionary<string, string>
			{
				{ "channelId", request.ChannelId },
				{ "address", request.Address },
				{ "feerateSatByte", request.FeerateSatByte.ToString() }
			});

			var response = await client.PostAsync("/closechannel", content);
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<CloseChannelResponse>(json);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error closing channel: {ex.Message}");
			return null;
		}
	}
	
	public async Task<DecodeInvoiceResponse?> DecodeInvoiceAsync(DecodeInvoiceRequest invoice)
	{
		try
		{
			var apiUrl = await _configurationService.GetApiUrlAsync();
			var apiPassword = await _configurationService.GetApiPasswordAsync();

			if (string.IsNullOrWhiteSpace(apiUrl) || string.IsNullOrWhiteSpace(apiPassword))
			{
				throw new InvalidOperationException("API URL or API password is missing.");
			}

			using var client = CreateHttpClient(apiUrl, apiPassword);

			var requestContent = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("invoice", invoice.Invoice)
			});

			var response = await client.PostAsync("/decodeinvoice", requestContent);
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			var serializedInvoice = JsonSerializer.Deserialize<DecodeInvoiceResponse>(json);

			serializedInvoice!.Address = invoice.Invoice;
			serializedInvoice.Amount = serializedInvoice.Amount.ToSats();

			return serializedInvoice;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error decoding invoice: {ex.Message}");
			return null;
		}
	}

	// PAYMENTSERVICE
	public async Task<Bolt11InvoiceResponse?> CreateInvoiceAsync(Bolt11InvoiceRequest request)
	{
		try
		{
			var apiUrl = await _configurationService.GetApiUrlAsync();
			var apiPassword = await _configurationService.GetApiPasswordAsync();

			if (string.IsNullOrWhiteSpace(apiUrl) || string.IsNullOrWhiteSpace(apiPassword))
			{
				throw new InvalidOperationException("API URL or API password is missing.");
			}

			using var client = CreateHttpClient(apiUrl, apiPassword);

			var content = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("description", request.Description),
				new KeyValuePair<string, string>("amountSat", request.AmountSat.ToString()),
				new KeyValuePair<string, string>("expirySeconds", request.ExpirySeconds.ToString()),
				new KeyValuePair<string, string>("externalId", request.ExternalId ?? ""),
				new KeyValuePair<string, string>("webhookUrl", request.WebhookUrl ?? "")
			});

			var response = await client.PostAsync("/createinvoice", content);
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<Bolt11InvoiceResponse>(json);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error creating invoice: {ex.Message}");
			return null;
		}
	}

	public async Task<GetBolt12OfferResponse?> GetBolt12OfferAsync()
	{
		try
		{
			var apiUrl = await _configurationService.GetApiUrlAsync();
			var apiPassword = await _configurationService.GetApiPasswordAsync();

			if (string.IsNullOrWhiteSpace(apiUrl) || string.IsNullOrWhiteSpace(apiPassword))
			{
				throw new InvalidOperationException("API URL or API password is missing.");
			}

			using var client = CreateHttpClient(apiUrl, apiPassword);

			var response = await client.GetAsync("/getoffer");
			response.EnsureSuccessStatusCode();

			var offer = await response.Content.ReadAsStringAsync();
			return new GetBolt12OfferResponse { Offer = offer };
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error fetching Bolt12 offer: {ex.Message}");
			return null;
		}
	}

	public async Task<GetLightningAddressResponse?> GetLightningAddressAsync()
	{
		try
		{
			var apiUrl = await _configurationService.GetApiUrlAsync();
			var apiPassword = await _configurationService.GetApiPasswordAsync();

			if (string.IsNullOrWhiteSpace(apiUrl) || string.IsNullOrWhiteSpace(apiPassword))
			{
				throw new InvalidOperationException("API URL or API password is missing.");
			}

			using var client = CreateHttpClient(apiUrl, apiPassword);

			var response = await client.GetAsync("/getlnaddress");
			response.EnsureSuccessStatusCode();

			var address = await response.Content.ReadAsStringAsync();
			return new GetLightningAddressResponse { LightningAddress = address };
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error fetching Lightning address: {ex.Message}");
			return null;
		}
	}

	public async Task<List<IncomingPayment>> GetIncomingPaymentsAsync(string? externalId = null, bool all = true, int limit = 20, int offset = 0)
	{
		try
		{
			var apiUrl = await _configurationService.GetApiUrlAsync();
			var apiPassword = await _configurationService.GetApiPasswordAsync();

			if (string.IsNullOrWhiteSpace(apiUrl) || string.IsNullOrWhiteSpace(apiPassword))
			{
				throw new InvalidOperationException("API URL or API password is missing.");
			}

			using var client = CreateHttpClient(apiUrl, apiPassword);

			var queryParams = $"?all={all}&limit={limit}&offset={offset}";
			if (!string.IsNullOrWhiteSpace(externalId))
			{
				queryParams += $"&externalId={externalId}";
			}

			var response = await client.GetAsync($"/payments/incoming{queryParams}");
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<List<IncomingPayment>>(json) ?? new List<IncomingPayment>();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error fetching incoming payments: {ex.Message}");
			return new List<IncomingPayment>();
		}
	}

	public async Task<List<OutgoingPayment>> GetOutgoingPaymentsAsync(bool all = true, int limit = 20, int offset = 0)
	{
		try
		{
			var apiUrl = await _configurationService.GetApiUrlAsync();
			var apiPassword = await _configurationService.GetApiPasswordAsync();

			if (string.IsNullOrWhiteSpace(apiUrl) || string.IsNullOrWhiteSpace(apiPassword))
			{
				throw new InvalidOperationException("API URL or API password is missing.");
			}

			using var client = CreateHttpClient(apiUrl, apiPassword);

			var queryParams = $"?all={all}&limit={limit}&offset={offset}";
			var response = await client.GetAsync($"/payments/outgoing{queryParams}");
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<List<OutgoingPayment>>(json) ?? new List<OutgoingPayment>();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error fetching outgoing payments: {ex.Message}");
			return new List<OutgoingPayment>();
		}
	}

	public async Task<PaymentResponse?> ProcessPaymentAsync(InvoiceType type, string data, DecodeInvoiceResponse? decodedInvoice)
	{
		if (decodedInvoice == null)
			throw new InvalidOperationException("Invalid payment data.");

		switch (type)
		{
			case InvoiceType.Bolt11Invoice:
				var bolt11Response = await PayBolt11InvoiceAsync(new PayBolt11Invoice
				{
					Invoice = data,
					AmountSat = decodedInvoice.Amount
				});
				return new PaymentResponse
				{
					RecipientAmountSat = bolt11Response.RecipientAmountSat,
					RoutingFeeSat = bolt11Response.RoutingFeeSat,
					PaymentId = bolt11Response.PaymentId,
					PaymentHash = bolt11Response.PaymentHash,
					PaymentPreimage = bolt11Response.PaymentPreimage
				};

			case InvoiceType.Bolt12Offer:
				var bolt12Response = await PayBolt12OfferAsync(new PayBolt12OfferRequest
				{
					Offer = data,
					AmountSat = decodedInvoice.Amount,
					Message = decodedInvoice.Description
				});
				return new PaymentResponse
				{
					RecipientAmountSat = bolt12Response.RecipientAmountSat,
					RoutingFeeSat = bolt12Response.RoutingFeeSat,
					PaymentId = bolt12Response.PaymentId,
					PaymentHash = bolt12Response.PaymentHash,
					PaymentPreimage = bolt12Response.PaymentPreimage
				};

			case InvoiceType.Lnurl:
				var lnUrlResponse = await LnUrlPayAsync(new LnUrlPayRequest
				{
					LnUrl = data,
					AmountSat = decodedInvoice.Amount,
					Message = decodedInvoice.Description
				});
				return new PaymentResponse
				{
					RecipientAmountSat = lnUrlResponse.RecipientAmountSat,
					RoutingFeeSat = lnUrlResponse.RoutingFeeSat,
					PaymentId = lnUrlResponse.PaymentId,
					PaymentHash = lnUrlResponse.PaymentHash,
					PaymentPreimage = lnUrlResponse.PaymentPreimage
				};

			case InvoiceType.LightningAddress:
				var lightningResponse = await PayLightningAddressAsync(new PayLightningAddressRequest
				{
					Address = data,
					AmountSat = decodedInvoice.Amount,
					Message = decodedInvoice.Description
				});
				return new PaymentResponse
				{
					RecipientAmountSat = lightningResponse.RecipientAmountSat,
					RoutingFeeSat = lightningResponse.RoutingFeeSat,
					PaymentId = lightningResponse.PaymentId,
					PaymentHash = lightningResponse.PaymentHash,
					PaymentPreimage = lightningResponse.PaymentPreimage
				};

			case InvoiceType.OnChainAddress:
				var onChainResponse = await PayOnChainAsync(new PayOnChainRequest
				{
					Address = data,
					AmountSat = decodedInvoice.Amount,
					FeerateSatByte = decodedInvoice.FeeRateOnChain,
					
				});
				return new PaymentResponse
				{
					TxId = onChainResponse.TransactionId,
				};

			default:
				throw new InvalidOperationException("Unsupported invoice type.");
		}
	}

	public async Task<PayBolt11InvoiceResponse?> PayBolt11InvoiceAsync(PayBolt11Invoice request)
	{
		try
		{
			var apiUrl = await _configurationService.GetApiUrlAsync();
			var apiPassword = await _configurationService.GetApiPasswordAsync();

			using var client = CreateHttpClient(apiUrl, apiPassword);

			var content = new FormUrlEncodedContent(new Dictionary<string, string?>
			{
				{ "amountSat", request.AmountSat?.ToString() },
				{ "invoice", request.Invoice }
			});

			var response = await client.PostAsync("/payinvoice", content);
			
			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<PayBolt11InvoiceResponse>(responseContent, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
			}
			else
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				throw new HttpRequestException($"Error: {response.StatusCode}, Content: {errorContent}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error processing payment: {ex.Message}");
			throw;
		}
	}

	public async Task<PayBolt12OfferResponse?> PayBolt12OfferAsync(PayBolt12OfferRequest request)
	{
		try
		{
			var apiUrl = await _configurationService.GetApiUrlAsync();
			var apiPassword = await _configurationService.GetApiPasswordAsync();

			using var client = CreateHttpClient(apiUrl, apiPassword);

			var content = new FormUrlEncodedContent(new Dictionary<string, string?>
			{
				{ "amountSat", request.AmountSat?.ToString() },
				{ "offer", request.Offer.TrimStart('\u20BF') },
				{ "message", request.Message }
			});

			var response = await client.PostAsync("/payoffer", content);

			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<PayBolt12OfferResponse>(responseContent, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
			}
			else
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				throw new HttpRequestException($"Error: {response.StatusCode}, Content: {errorContent}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error processing Bolt12 offer payment: {ex.Message}");
			throw;
		}
	}

	public async Task<LnUrlPayResponse> LnUrlPayAsync(LnUrlPayRequest request)
	{
		try
		{
			var apiUrl = await _configurationService.GetApiUrlAsync();
			var apiPassword = await _configurationService.GetApiPasswordAsync();

			using var client = CreateHttpClient(apiUrl, apiPassword);

			var content = new FormUrlEncodedContent(new Dictionary<string, string?>
			{
				{ "amountSat", request.AmountSat.ToString() },
				{ "lnurl", request.LnUrl },
				{ "message", request.Message }
			});

			var response = await client.PostAsync("/lnurlpay", content);

			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<LnUrlPayResponse>(responseContent, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				})!;
			}
			else
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				throw new HttpRequestException($"Error: {response.StatusCode}, Content: {errorContent}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error processing LNURL Pay: {ex.Message}");
			throw;
		}
	}

	public async Task<PayLightningAddressResponse> PayLightningAddressAsync(PayLightningAddressRequest request)
	{
		try
		{
			var apiUrl = await _configurationService.GetApiUrlAsync();
			var apiPassword = await _configurationService.GetApiPasswordAsync();

			using var client = CreateHttpClient(apiUrl, apiPassword);

			var content = new FormUrlEncodedContent(new Dictionary<string, string?>
			{
				{ "amountSat", request.AmountSat?.ToString() },
				{ "address", request.Address },
				{ "message", request.Message }
			});

			var response = await client.PostAsync("/paylnaddress", content);

			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<PayLightningAddressResponse>(responseContent, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				})!;
			}
			else
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				throw new HttpRequestException($"Error: {response.StatusCode}, Content: {errorContent}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error processing Lightning address payment: {ex.Message}");
			throw;
		}
	}

	public async Task<PayOnChainResponse> PayOnChainAsync(PayOnChainRequest request)
	{
		try
		{
			var apiUrl = await _configurationService.GetApiUrlAsync();
			var apiPassword = await _configurationService.GetApiPasswordAsync();

			if (string.IsNullOrWhiteSpace(apiUrl) || string.IsNullOrWhiteSpace(apiPassword))
			{
				throw new InvalidOperationException("API URL or API password is missing.");
			}

			using var client = CreateHttpClient(apiUrl, apiPassword);

			var content = new FormUrlEncodedContent(new Dictionary<string, string>
			{
				{ "amountSat", request.AmountSat.ToString()! },
				{ "address", request.Address! },
				{ "feerateSatByte", request.FeerateSatByte.ToString() }
			});

			var response = await client.PostAsync("/sendtoaddress", content);

			if (response.IsSuccessStatusCode)
			{
				var transactionId = await response.Content.ReadAsStringAsync();
				return new PayOnChainResponse { TransactionId = transactionId };
			}
			else
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				throw new HttpRequestException($"Error: {response.StatusCode}, Content: {errorContent}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error sending to address: {ex.Message}");
			throw;
		}
	}


	// LNURL SERVICE
	public async Task<string?> LnUrlAuthAsync(string lnurl)
	{
		try
		{
			var apiUrl = await _configurationService.GetApiUrlAsync();
			var apiPassword = await _configurationService.GetApiPasswordAsync();

			if (string.IsNullOrWhiteSpace(apiUrl) || string.IsNullOrWhiteSpace(apiPassword))
			{
				throw new InvalidOperationException("API URL or API password is missing.");
			}

			using var client = CreateHttpClient(apiUrl, apiPassword);

			var content = new FormUrlEncodedContent(new Dictionary<string, string>
			{
				{ "lnurl", lnurl }
			});

			var response = await client.PostAsync("/lnurlauth", content);

			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync();
				return responseContent;
			}
			else
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				throw new HttpRequestException($"Error: {response.StatusCode}, Content: {errorContent}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error during LNURL Auth: {ex.Message}");
			throw;
		}
	}


}
