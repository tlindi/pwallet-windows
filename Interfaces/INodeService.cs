using pWallet.Models;

namespace pWallet.Interfaces;

public interface INodeService
{
	Task<NodeInfoResponse?> GetNodeInfoAsync();
	Task<BalanceResponse?> GetBalanceAsync();
	Task<CloseChannelResponse?> CloseChannelAsync(CloseChannelRequest request);
	Task<DecodeInvoiceResponse?> DecodeInvoiceAsync(DecodeInvoiceRequest invoice);
}
