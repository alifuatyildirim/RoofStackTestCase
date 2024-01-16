using Microsoft.AspNetCore.Mvc;
using Wallet.Common.Mediatr.Query;
using Wallet.Contract; 
using Wallet.Contract.Query.Wallet;
using Wallet.Contract.Query.WalletTransaction;
using Wallet.Contract.Response.Wallet;
using Wallet.Contract.Response.WalletTransaction;

namespace Wallet.Api.Controllers;

public class WalletTransactionQueryController: ControllerBase
{
    private readonly IQueryProcessor queryProcessor;
    public WalletTransactionQueryController(IQueryProcessor queryProcessor)
    {
        this.queryProcessor = queryProcessor;
    }
        
    [Route("walletTransaction/getAll/{walletId:Guid}")]
    [HttpGet]
    public async Task<ApiResponse<WalletTransactionResponse>> GetWalletTransaction([FromRoute]GetAllWalletTransactionQuery query)
    {
        return new ApiResponse<WalletTransactionResponse>
        {
            Data = await queryProcessor.ProcessAsync(query),
        };
    }
}