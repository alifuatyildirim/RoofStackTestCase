using Wallet.Domain.Entities.BaseEntity;

namespace Wallet.Domain.Entities;

public class TestEntity : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}