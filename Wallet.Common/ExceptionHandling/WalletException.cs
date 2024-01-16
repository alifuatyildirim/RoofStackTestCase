using System.Net;
using Microsoft.Extensions.Logging;
using Wallet.Common.Codes;

namespace Wallet.Common.ExceptionHandling
{
    public class WalletException : Exception
    {
        public WalletException(
            ErrorMessage errorMessage,
            string message,
            HttpStatusCode? httpStatusCode = null,
            Exception? innerException = null,
            LogLevel logLevel = Microsoft.Extensions.Logging.LogLevel.Trace,
            EventId? eventId = null)
            : base(message, innerException)
        {
            this.ErrorMessage = errorMessage;
            this.HttpStatusCode = httpStatusCode;
            this.LogLevel = logLevel;
            this.EventId = eventId;
        }

        public WalletException(
            ErrorCode errorCode,
            string message,
            HttpStatusCode? httpStatusCode = null,
            Exception? innerException = null,
            LogLevel logLevel = Microsoft.Extensions.Logging.LogLevel.Trace,
            EventId? eventId = null)
            : base(message, innerException)
        {
            this.ErrorMessage = errorCode.CreateMessage();
            this.HttpStatusCode = httpStatusCode;
            this.LogLevel = logLevel;
            this.EventId = eventId;
        }

        public ErrorMessage? ErrorMessage { get; }

        public HttpStatusCode? HttpStatusCode { get; }

        public LogLevel? LogLevel { get; }

        public EventId? EventId { get; }

        public string? LogMessage { get; set; }
        public object? ErrorData { get; protected set; }
    }
}
