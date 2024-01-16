using Wallet.Application.Services.Abstraction;
using Wallet.Common.Mediatr.Query; 
using Wallet.Contract.Query.Wallet; 
using Wallet.Contract.Response.Wallet;

namespace Wallet.Application.Handlers.Wallet;

public class GetWalletByUserQueryHandler : IQueryHandler<GetWalletByUserQuery, WalletResponse>
{

    private readonly IWalletService walletService;
    public GetWalletByUserQueryHandler(IWalletService walletService)
    {
        this.walletService = walletService;
    }
    public async Task<WalletResponse> Handle(GetWalletByUserQuery request, CancellationToken cancellationToken)
    {
        return await this.walletService.GetWalletAsync(request.UserId, cancellationToken);
    }
}