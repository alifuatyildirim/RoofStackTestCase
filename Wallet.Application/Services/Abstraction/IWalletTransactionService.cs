using Wallet.Contract; 
using Wallet.Contract.Command.WalletTransaction; 
using Wallet.Contract.Query.WalletTransaction; 
using Wallet.Contract.Response.WalletTransaction;

namespace Wallet.Application.Services.Abstraction;

public interface IWalletTransactionService : IApplicationService
{
    Task<GuidIdResult> CreateWalletTransactionAsync(CreateWalletTransactionCommand command, CancellationToken cancellationToken);
    Task<WalletTransactionResponse> GetAllWalletTransactionsAsync(GetAllWalletTransactionQuery request, CancellationToken cancellationToken);
}