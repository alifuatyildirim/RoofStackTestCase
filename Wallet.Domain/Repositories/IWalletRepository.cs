using Wallet.Common.Abstraction; 

namespace Wallet.Domain.Repositories;

public interface IWalletRepository : IRepository
{
    Task<Guid> CreateAsync(Domain.Entities.Wallet wallet, ITransactionScope? scope = null);
    Task<Guid> UpdateAsync(Domain.Entities.Wallet wallet, ITransactionScope? scope = null);
    Task<IReadOnlyList<Domain.Entities.Wallet>> GetByUserIdAsync(Guid userId);
    Task<Domain.Entities.Wallet> GetAsync(Guid Id);
}