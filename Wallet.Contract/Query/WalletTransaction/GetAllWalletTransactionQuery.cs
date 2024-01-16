using Wallet.Common.Mediatr.Query;
using Wallet.Contract.Response.WalletTransaction;

namespace Wallet.Contract.Query.WalletTransaction;

public class GetAllWalletTransactionQuery: IQuery<WalletTransactionResponse>
{
    public Guid WalletId { get; set; }
}