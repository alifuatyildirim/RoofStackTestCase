using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.Base;

namespace Wallet.Data.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly IGenericRepository<Domain.Entities.Wallet, Guid> genericRepository;

    public WalletRepository(IGenericRepository<Domain.Entities.Wallet, Guid> genericRepository)
    {
        this.genericRepository = genericRepository;
        this.genericRepository.CreateCollection();

    }
    
    public async Task<Guid> CreateAsync(Domain.Entities.Wallet wallet, ITransactionScope? scope)
    {
        wallet.Id = Guid.NewGuid();
        await this.genericRepository.AddOneAsync(wallet,transactionScope:scope);
        return wallet.Id;
    }

    public async Task<Guid> UpdateAsync(Domain.Entities.Wallet wallet, ITransactionScope? scope = null)
    {
        await this.genericRepository.UpdateAsync(wallet,scope);
        return wallet.Id;
    }

    public async Task<IReadOnlyList<Domain.Entities.Wallet>> GetByUserIdAsync(Guid userId)
    {
       return await this.genericRepository.Query().Where(x=>x.UserId == userId).ToListAsync();
    }

    public async Task<Domain.Entities.Wallet> GetAsync(Guid Id)
    {
        return await this.genericRepository.GetByIdAsync(Id);
    }
}