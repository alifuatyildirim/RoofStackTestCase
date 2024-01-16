using Wallet.Common.Enums;

namespace Wallet.Contract.Response.WalletTransaction;

public record WalletTransactionResponseItem
{
    public Guid WalletId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; } 
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; } 
}