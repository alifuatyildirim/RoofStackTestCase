using System.Net.Mime;
using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Wallet.Common.ExceptionHandling;

namespace Wallet.Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly Func<object, Task> clearCacheHeadersDelegate;

        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
            this.clearCacheHeadersDelegate = ClearCacheHeadersAsync;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "ErrorHandlingMiddleware should catch all errors!")]
        public async Task InvokeAsync(HttpContext context)
        {
            ExceptionDispatchInfo edi;

            try
            {
                Task task = this.next(context);

                if (!task.IsCompletedSuccessfully)
                {
                    await Awaited(this, context, task);
                }

                return;
            }
            catch (Exception exception)
            { 
                edi = ExceptionDispatchInfo.Capture(exception);
            }

            await this.HandleExceptionAsync(context, edi);

            static async Task Awaited(ExceptionHandlerMiddleware middleware, HttpContext context, Task task)
            {
                ExceptionDispatchInfo? edi = null;

                try
                {
                    await task;
                }
                catch (Exception exception)
                { 
                    edi = ExceptionDispatchInfo.Capture(exception);
                }

                if (edi == null)
                {
                    return;
                }

                await middleware.HandleExceptionAsync(context, edi);
            }
        }

        private static void ClearHttpContext(HttpContext context)
        {
            context.Response.Clear();
 
            context.SetEndpoint(null);
            IRouteValuesFeature? routeValuesFeature = context.Features.Get<IRouteValuesFeature>();
            routeValuesFeature?.RouteValues.Clear();
        }

        private static Task ClearCacheHeadersAsync(object state)
        {
            IHeaderDictionary headers = ((HttpResponse)state).Headers;
            headers[HeaderNames.CacheControl] = "no-cache";
            headers[HeaderNames.Pragma] = "no-cache";
            headers[HeaderNames.Expires] = "-1";
            headers.Remove(HeaderNames.ETag);
            return Task.CompletedTask;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "ErrorHandlingMiddleware should catch all errors!")]
        private async Task HandleExceptionAsync(HttpContext context, ExceptionDispatchInfo edi)
        {
            PathString originalPath = context.Request.Path;
 
            if (context.Response.HasStarted)
            {
                this.logger.LogError(edi.SourceException, $"An unhandled exception was thrown by the application. Path:{originalPath}, TraceId:{context.TraceIdentifier}");

                this.logger.LogWarning(edi.SourceException, "The response has already started, the error handler will not be executed.");

                var error = new Dictionary<string, object?>
                {
                    ["data"] = null,
                    ["message"] = edi.SourceException.Message,
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
                return;
            }

            try
            {
                var walletException = (WalletException)edi.SourceException;

                if (context.Response.HasStarted)
                {
                    ClearHttpContext(context);
                }

                int httpStatusCode = 0;
                if (walletException.HttpStatusCode != null)
                {
                    httpStatusCode = (int)walletException.HttpStatusCode;
                }
                else if (!context.Response.HasStarted && context.Response.StatusCode == 200)
                {
                    httpStatusCode = 500;
                }
                else
                {
                    httpStatusCode = context.Response.StatusCode;
                }

                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = httpStatusCode;
                var exceptionHandlerFeature = new ExceptionHandlerFeature()
                {
                    Error = edi.SourceException,
                    Path = originalPath.Value ?? string.Empty,
                };
                context.Features.Set<IExceptionHandlerFeature>(exceptionHandlerFeature);
                context.Features.Set<IExceptionHandlerPathFeature>(exceptionHandlerFeature);
                context.Response.OnStarting(this.clearCacheHeadersDelegate, context.Response);

                var error = new Dictionary<string, object?>
                {
                    ["data"] = walletException.ErrorData,
                    ["message"] = walletException.ErrorMessage?.Message,
                    ["errorCode"] = walletException.ErrorMessage?.Code,
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(error));

                return;
            }
            catch (Exception ex2)
            {
                this.logger.LogError(ex2, "An exception was thrown attempting to execute the error handler.");
            }
            finally
            {
                context.Request.Path = originalPath;
            }

            edi.Throw();
        }
    }
}
