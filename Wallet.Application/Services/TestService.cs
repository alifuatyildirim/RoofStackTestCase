using MapsterMapper;
using Wallet.Application.Services.Abstraction;
using Wallet.Contract.Query.Test;
using Wallet.Domain.Entities;
using Wallet.Domain.Repositories;

namespace Wallet.Application.Services;

public class TestService : ITestService
{
    private readonly ITestRepository testRepository;
    private readonly IMapper mapper;

    public TestService(ITestRepository testRepository,IMapper mapper)
    {
        this.testRepository = testRepository;
        this.mapper = mapper;
    }
    public async Task<Guid> Test(GetTestQuery query)
    {
        var entity = this.mapper.Map<TestEntity>(query);
        return await testRepository.CreateAsync(entity);
    }
}