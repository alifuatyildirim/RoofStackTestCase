using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Wallet.Api.Integration.Test.Extensions
{
    public static class ObjectExtensions
    {
        public static void AssertNotNull([NotNull] this object? o)
        {
            Assert.NotNull(o);

            if (!(o is null))
            {
                return;
            }

            throw new InvalidOperationException("It was null!");
        }
    }
}
