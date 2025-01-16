using pWallet.Enums;

namespace pWallet.Models;

public class InvoiceSubmission
{
	public string Invoice { get; set; } = string.Empty;
	public InvoiceType Type { get; set; } = InvoiceType.Unknown;
}
