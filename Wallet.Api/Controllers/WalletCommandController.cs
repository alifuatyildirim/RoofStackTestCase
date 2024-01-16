using Microsoft.AspNetCore.Mvc;
using Wallet.Common.Mediatr.Command;
using Wallet.Contract;
using Wallet.Contract.Command.Wallet;

namespace Wallet.Api.Controllers;

public class WalletCommandController : ControllerBase
{
    private readonly IApplicationCommandSender commandSender;
    public WalletCommandController(IApplicationCommandSender commandSender)
    {
        this.commandSender = commandSender;
    }

    [Route("wallet/create/{userId:Guid}")]
    [HttpPost]
    public async  Task<ApiResponse<GuidIdResult>> CreateWallet([FromBody]CreateWalletCommand command, Guid userId)
    {
        command.UserId = userId;
        return new ApiResponse<GuidIdResult>
        {
            Data = await this.commandSender.SendAsync(command),
        };
    }
}