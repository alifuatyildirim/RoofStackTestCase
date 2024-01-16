using Wallet.Common.Abstraction; 

namespace Wallet.Domain.Repositories;

public interface IWalletRepositoryForTest : IRepository
{
    Task<IReadOnlyList<Domain.Entities.Wallet>> GetAllAsync();
}