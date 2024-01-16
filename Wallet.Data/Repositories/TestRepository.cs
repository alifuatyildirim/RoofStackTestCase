using Wallet.Domain.Entities;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.Base;

namespace Wallet.Data.Repositories;

public class TestRepository : ITestRepository
{
    private readonly IGenericRepository<TestEntity, Guid> genericRepository;

    public TestRepository(IGenericRepository<TestEntity, Guid> genericRepository)
    {
        this.genericRepository = genericRepository;
        this.genericRepository.CreateCollection();

    }
    public async Task<Guid> CreateAsync(TestEntity testEntity, ITransactionScope? scope)
    {
        testEntity.Id = Guid.NewGuid();
        await this.genericRepository.AddOneAsync(testEntity,transactionScope:scope);
        return testEntity.Id;
    }
}