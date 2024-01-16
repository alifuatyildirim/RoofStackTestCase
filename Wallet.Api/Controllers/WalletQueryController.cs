using Microsoft.AspNetCore.Mvc;
using Wallet.Common.Mediatr.Query;
using Wallet.Contract; 
using Wallet.Contract.Query.Wallet; 
using Wallet.Contract.Response.Wallet;

namespace Wallet.Api.Controllers;

public class WalletQueryController: ControllerBase
{
    private readonly IQueryProcessor queryProcessor;
    public WalletQueryController(IQueryProcessor queryProcessor)
    {
        this.queryProcessor = queryProcessor;
    }
        
    [Route("wallet/{userId:Guid}")]
    [HttpGet]
    public async Task<ApiResponse<WalletResponse>> GetWalletByUser([FromRoute]GetWalletByUserQuery query)
    {
        return new ApiResponse<WalletResponse>
        {
            Data = await queryProcessor.ProcessAsync(query),
        };
    }
}