using Wallet.Common.Enums;

namespace Wallet.Contract.Response.Wallet;

public class WalletResponseItem
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Limit { get; set; }
    public CurrencyCode CurrencyCode { get; set; }
}