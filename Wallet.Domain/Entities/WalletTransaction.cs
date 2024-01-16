using Wallet.Common.Enums;
using Wallet.Domain.Entities.BaseEntity;

namespace Wallet.Domain.Entities;

public class WalletTransaction : IEntity<Guid>
{
    public Guid Id { get; set; } 
    public Guid WalletId { get; set; } 
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public Guid CreatedBy { get; set; } 
}