using Wallet.Contract.Query.Test;

namespace Wallet.Application.Services.Abstraction;

public interface ITestService:IApplicationService
{
    Task<Guid> Test(GetTestQuery request);
}