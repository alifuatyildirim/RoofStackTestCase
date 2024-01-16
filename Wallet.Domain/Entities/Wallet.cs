using Wallet.Common.Enums;
using Wallet.Domain.Entities.BaseEntity;

namespace Wallet.Domain.Entities;

public class Wallet : IEntity<Guid>
{
    public Guid Id { get; set; } 
    public Guid UserId { get; set; }
    public CurrencyCode CurrencyCode { get; set; }
    public decimal Limit { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow; 
}