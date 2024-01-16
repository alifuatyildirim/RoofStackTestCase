using Wallet.Application.Services.Abstraction;
using Wallet.Common.Mediatr.Query;
using Wallet.Contract.Query.Wallet;
using Wallet.Contract.Query.WalletTransaction;
using Wallet.Contract.Response.Wallet;
using Wallet.Contract.Response.WalletTransaction;

namespace Wallet.Application.Handlers.WalletTransaction;

public class GetAllWalletTransactionQueryHandler : IQueryHandler<GetAllWalletTransactionQuery, WalletTransactionResponse>
{

    private readonly IWalletTransactionService walletTransactionService;
    public GetAllWalletTransactionQueryHandler(IWalletTransactionService walletTransactionService)
    {
        this.walletTransactionService = walletTransactionService;
    }
    public async Task<WalletTransactionResponse> Handle(GetAllWalletTransactionQuery request, CancellationToken cancellationToken)
    {
        return await this.walletTransactionService.GetAllWalletTransactionsAsync(request, cancellationToken);
    }
}