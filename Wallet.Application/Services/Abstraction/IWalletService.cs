using Wallet.Contract;
using Wallet.Contract.Command.Wallet;
using Wallet.Contract.Command.WalletTransaction;
using Wallet.Contract.Response.Wallet;
using Wallet.Domain.Repositories;

namespace Wallet.Application.Services.Abstraction;

public interface IWalletService : IApplicationService
{
    Task<GuidIdResult> CreateWalletAsync(CreateWalletCommand command, CancellationToken cancellationToken);
    Task<WalletResponse> GetWalletAsync(Guid userId, CancellationToken cancellationToken);

    Task UpdateWalletAsync(CreateWalletTransactionCommand command, ITransactionScope? transactionScope);
}