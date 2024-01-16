using Wallet.Application.Services.Abstraction;
using Wallet.Common.Mediatr.Command;
using Wallet.Contract;
using Wallet.Contract.Command.Wallet;

namespace Wallet.Application.Handlers.Wallet
{
    public class CreateWalletCommandHandler : IApplicationCommandHandler<CreateWalletCommand, GuidIdResult>
    {
        private readonly IWalletService walletService;

        public CreateWalletCommandHandler(IWalletService walletService)
        {
            this.walletService = walletService;
        }

        public async Task<GuidIdResult> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            return await this.walletService.CreateWalletAsync(request, cancellationToken);
        }
    }
}
