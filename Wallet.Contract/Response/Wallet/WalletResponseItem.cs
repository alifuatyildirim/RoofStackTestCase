using Wallet.Common.Enums;

namespace Wallet.Contract.Response.Wallet;

public record WalletResponseItem
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid WalletNumber { get; set; }
    public decimal Limit { get; set; }
    public CurrencyCode CurrencyCode { get; set; }
}