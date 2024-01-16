using Wallet.Common.Mediatr.Query;
using Wallet.Contract.Response.Wallet;

namespace Wallet.Contract.Query.Wallet;

public record GetWalletByUserQuery: IQuery<WalletResponse>
{
    public Guid UserId { get; set; }
}