using Wallet.Common.Mediatr.Query;
using Wallet.Contract.Response.Test;

namespace Wallet.Contract.Query.Test;

public class GetTestQuery : IQuery<TestResponse>
{
    public Guid Id { get; set; }
}