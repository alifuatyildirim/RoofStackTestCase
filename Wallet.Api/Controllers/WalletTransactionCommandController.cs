using Microsoft.AspNetCore.Mvc;
using Wallet.Common.Mediatr.Command;
using Wallet.Contract;
using Wallet.Contract.Command.Wallet;
using Wallet.Contract.Command.WalletTransaction;

namespace Wallet.Api.Controllers;

public class WalletTransactionCommandController : ControllerBase
{
    private readonly IApplicationCommandSender commandSender;
    public WalletTransactionCommandController(IApplicationCommandSender commandSender)
    {
        this.commandSender = commandSender;
    }

    [Route("walletTransaction/create/{walletId:Guid}")]
    [HttpPost]
    public async  Task<ApiResponse<GuidIdResult>> CreateWalletTransaction([FromBody]CreateWalletTransactionCommand command,Guid walletId)
    {
        command.WalletId = walletId;
        return new ApiResponse<GuidIdResult>
        {
            Data = await this.commandSender.SendAsync(command),
        };
    }
}