using Newtonsoft.Json;
using Wallet.Common.Enums;
using Wallet.Common.Mediatr.Command;

namespace Wallet.Contract.Command.Wallet;

public class CreateWalletCommand : IApplicationCommand<GuidIdResult>
{
    [JsonIgnore]
    public Guid UserId { get; set; } 
    public CurrencyCode CurrencyCode { get; set; }
}