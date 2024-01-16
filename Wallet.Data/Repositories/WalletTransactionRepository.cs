using Wallet.Domain.Entities;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.Base;

namespace Wallet.Data.Repositories;

public class WalletTransactionRepository : IWalletTransactionRepository
{
    private readonly IGenericRepository<WalletTransaction, Guid> genericRepository;

    public WalletTransactionRepository(IGenericRepository<WalletTransaction, Guid> genericRepository)
    {
        this.genericRepository = genericRepository;
        this.genericRepository.CreateCollection();

    }
    
    public async Task<Guid> CreateAsync(WalletTransaction walletTransaction, ITransactionScope? scope)
    {
        if (walletTransaction.Id == default)
        {
            walletTransaction.Id = Guid.NewGuid();
        }

        await this.genericRepository.AddOneAsync(walletTransaction,transactionScope:scope);
        return walletTransaction.Id;
    }

    public async Task<(IReadOnlyCollection<WalletTransaction> Items, int Count)> GetAllTransactions(Guid walletId)
    {
        IReadOnlyCollection<WalletTransaction> result = await this.genericRepository.Query().Where(x => x.WalletId == walletId).ToListAsync();
        return (result, result.Count);
    } 
    
    public async Task<ITransactionScope> BeginTransactionScopeAsync()
    {
        ITransactionScope transactionScope = await this.genericRepository.BeginTransactionScopeAsync();

        transactionScope.BeginTransaction();

        return transactionScope;

    }
}