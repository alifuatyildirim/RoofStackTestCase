using Newtonsoft.Json;
using Wallet.Contract.Abstraction;
using Wallet.Contract.Response.Abstraction;

namespace Wallet.Contract
{
    public class GuidIdResult : IdResult<Guid>
    {
        [JsonConstructor]
        public GuidIdResult(in Guid id)
        {
            this.Id = id;
        }
    }
}
