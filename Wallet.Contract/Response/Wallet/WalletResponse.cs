namespace Wallet.Contract.Response.Wallet;

public record WalletResponse
{
    public IReadOnlyCollection<WalletResponseItem> Items { get; set; }
}