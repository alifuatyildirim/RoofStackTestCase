using Microsoft.Extensions.Options;
using NSubstitute;

namespace Wallet.Api.Integration.Test.Setup.Substitution
{
    public class SubstituteOptions<TOptions> : IOptions<TOptions>
        where TOptions : class, new()
    {
        public TOptions Value => Substitute.For<TOptions>();
    }
}
