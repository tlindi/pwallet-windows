using pWallet.Enums;
using pWallet.Models;

namespace pWallet.Interfaces;

public interface IPaymentService
{
	Task<Bolt11InvoiceResponse?> CreateInvoiceAsync(Bolt11InvoiceRequest request);
	Task<GetBolt12OfferResponse?> GetBolt12OfferAsync();
	Task<GetLightningAddressResponse?> GetLightningAddressAsync();
	Task<List<IncomingPayment>> GetIncomingPaymentsAsync(string? externalId = null, bool all = true, int limit = 20, int offset = 0);
	Task<List<OutgoingPayment>> GetOutgoingPaymentsAsync(bool all = true, int limit = 20, int offset = 0);
	Task<PaymentResponse?> ProcessPaymentAsync(InvoiceType type, string data, DecodeInvoiceResponse? decodedInvoice);
	Task<PayBolt11InvoiceResponse?> PayBolt11InvoiceAsync(PayBolt11Invoice request);
}
