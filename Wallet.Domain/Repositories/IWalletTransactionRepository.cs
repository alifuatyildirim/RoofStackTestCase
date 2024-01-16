using Wallet.Common.Abstraction;
using Wallet.Domain.Entities;

namespace Wallet.Domain.Repositories;

public interface IWalletTransactionRepository : IRepository
{
    Task<Guid> CreateAsync(WalletTransaction walletTransaction, ITransactionScope? scope = null);
    Task<(IReadOnlyCollection<WalletTransaction> Items, int Count)> GetAllTransactions(Guid walletId);
    Task<ITransactionScope> BeginTransactionScopeAsync();
}