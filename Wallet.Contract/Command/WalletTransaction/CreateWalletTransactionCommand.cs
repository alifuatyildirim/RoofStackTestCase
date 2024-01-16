using Newtonsoft.Json;
using Wallet.Common.Enums;
using Wallet.Common.Mediatr.Command;

namespace Wallet.Contract.Command.WalletTransaction;

public class CreateWalletTransactionCommand : IApplicationCommand<GuidIdResult>
{ 
    [JsonIgnore]
    public Guid WalletId { get; set; }
    public CurrencyCode CurrencyCode { get; set; }
    public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; }
    public Guid CreatedBy { get; set; }
}