using System.Net;
using System.Runtime.ExceptionServices;
using Wallet.Common.Codes;
using Wallet.Common.ExceptionHandling;
using Wallet.Common.Mediatr.Exceptions;

namespace Wallet.Api.Extensions
{
    public static class ExceptionDispatchInfoExtensions
    {
        public static WalletException MapToWalletException<TLogger>(this ExceptionDispatchInfo edi, ILogger<TLogger> logger)
        {
            switch (edi.SourceException)
            {
                case WalletException walletEx:
                    LogLevel logLevel = walletEx.LogLevel ?? LogLevel.Error;
                    string message = walletEx.Message;
                    if (!string.IsNullOrEmpty(walletEx.LogMessage))
                    {
                        message = walletEx.LogMessage;
                    }
                    logger.Log(logLevel, walletEx.EventId ?? 0, walletEx, "A WalletException was thrown by the application: {Message}", message);
                    return walletEx;
                
                case CqrsValidationException validationEx:
                    WalletException vEx = new(ErrorCode.InvalidRequest.CreateMessage(validationEx.Message), validationEx.Message, System.Net.HttpStatusCode.BadRequest, logLevel: LogLevel.Information);
                    logger.LogInformation(vEx.EventId ?? 0, vEx, "A CqrsValidationException was thrown by the application: " + vEx.Message);
                    return vEx;

                default:
                    logger.LogError(edi.SourceException, $"An unhandled exception was thrown by the application: {edi.SourceException.Message}");
                    return new WalletException(ErrorCode.GenericError, $"An unhandled exception was thrown by the application: {edi.SourceException.Message}", HttpStatusCode.InternalServerError, edi.SourceException);
            }
        }
    }
}