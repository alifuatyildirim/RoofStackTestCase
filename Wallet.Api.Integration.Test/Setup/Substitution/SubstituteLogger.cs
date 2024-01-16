using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit.Abstractions;

namespace Wallet.Api.Integration.Test.Setup.Substitution
{
    public class SubstituteLogger<T> : ILogger<T>
    {
        private readonly ITestOutputHelper testOutputHelper;

        public SubstituteLogger(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            this.testOutputHelper.WriteLine("BeginScope: " + state);
            return Substitute.For<IDisposable>();
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
        {
            this.testOutputHelper.WriteLine($"Log ({logLevel}): {state}. {exception?.ToString()}");
        }
    }
}
