using System.Text.RegularExpressions;
using pWallet.Enums;

namespace pWallet.Utilities;

public static class InvoiceTypeUtility
{
	public static InvoiceType DetermineInvoiceType(string qrContent)
	{
		if (qrContent.StartsWith("lnbc") || qrContent.StartsWith("lntb"))
			return InvoiceType.Bolt11Invoice;
		if (qrContent.Contains("lno"))
			return InvoiceType.Bolt12Offer;
		if (qrContent.Contains("@"))
			return InvoiceType.LightningAddress;
		if (Regex.IsMatch(qrContent, @"^[13][a-km-zA-HJ-NP-Z1-9]{25,34}$") || Regex.IsMatch(qrContent, @"^bc1[ac-hj-np-z02-9]{11,71}$"))
			return InvoiceType.OnChainAddress;
		if (qrContent.Contains("lnurl"))
			return InvoiceType.Lnurl;

		return InvoiceType.Unknown;
	}
}
