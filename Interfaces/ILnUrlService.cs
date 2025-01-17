namespace pWallet.Interfaces;

public interface ILnUrlService
{
	Task<string?> LnUrlAuthAsync(string lnurl);
}
