using Wallet.Application.Services.Abstraction;
using Wallet.Common.Mediatr.Query;
using Wallet.Contract.Query.Test;
using Wallet.Contract.Response.Test;

namespace Wallet.Application.Handlers.Test;

public class GetTestQueryHandler : IQueryHandler<GetTestQuery, TestResponse>
{

    private readonly ITestService testService;
    public GetTestQueryHandler(ITestService testService)
    {
        this.testService = testService;
    }
    public async Task<TestResponse> Handle(GetTestQuery request, CancellationToken cancellationToken)
    {
        return new TestResponse
        {
            Id = await this.testService.Test(request)
        };
    }
}