using Wallet.Application.Services.Abstraction;
using Wallet.Common.Mediatr.Command;
using Wallet.Contract; 
using Wallet.Contract.Command.WalletTransaction;

namespace Wallet.Application.Handlers.WalletTransaction
{
    public class CreateWalletTransactionCommandHandler : IApplicationCommandHandler<CreateWalletTransactionCommand, GuidIdResult>
    {
        private readonly IWalletTransactionService walletTransactionService;

        public CreateWalletTransactionCommandHandler(IWalletTransactionService walletTransactionService)
        {
            this.walletTransactionService = walletTransactionService;
        }

        public async Task<GuidIdResult> Handle(CreateWalletTransactionCommand request, CancellationToken cancellationToken)
        {
            return await this.walletTransactionService.CreateWalletTransactionAsync(request, cancellationToken);
        }
    }
}
