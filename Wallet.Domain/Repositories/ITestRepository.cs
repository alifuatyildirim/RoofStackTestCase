using Wallet.Common.Abstraction;
using Wallet.Domain.Entities;

namespace Wallet.Domain.Repositories;

public interface ITestRepository : IRepository
{
    Task<Guid> CreateAsync(TestEntity testEntity, ITransactionScope? scope = null);
}