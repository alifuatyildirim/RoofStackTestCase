using System.Text;
using Microsoft.AspNetCore.Http.Extensions;

namespace Wallet.Api.Middlewares
{
    public class HttpLoggingMiddleware
    {
        private const LogLevel MiddlewareLogLevel = LogLevel.Information;

        private static readonly string[] AllowedAuthorizePage = new string[]
        {
            "/", "/index.html", "/swagger/v1/swagger.json",
        };

        private readonly ILogger<HttpLoggingMiddleware> logger;
        private readonly RequestDelegate next;

        public HttpLoggingMiddleware(ILogger<HttpLoggingMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string userName = context.Request.Headers["UserName"];
            using IDisposable? disposable = this.logger.BeginScope("{Username}", userName);

            string path = context.Request.Path.ToString();

            if (AllowedAuthorizePage.Contains(path))
            {
                await this.next(context);
                return;
            }

            if (!this.logger.IsEnabled(MiddlewareLogLevel))
            {
                await this.next(context);
                return;
            }

            string request = string.Empty;

            if (context.Request.HasFormContentType)
            {
                request = "Form content..";
            }
            else
            {
                request = await FormatRequestAsync(context.Request);
            }

            this.logger.Log(
                MiddlewareLogLevel,
                $"About to start {context.Request.Method} {context.Request.GetDisplayUrl()}. RequestBody: {request}");

            await this.FormatResponseAsync(context);
        }

        private static async Task<string> FormatRequestAsync(HttpRequest request)
        {
            string requestBody = string.Empty;

            using (var reader = new StreamReader(request.Body))
            {
                requestBody = await reader.ReadToEndAsync();

                byte[] bodyData = Encoding.UTF8.GetBytes(requestBody);

                request.Body = new MemoryStream(bodyData);
            }

            return requestBody;
        }

        private async Task FormatResponseAsync(HttpContext context)
        {
            Stream originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await this.next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(context.Response.Body))
                {
                    string text;

                    text = await reader.ReadToEndAsync();
                    context.Response.Body.Seek(0, SeekOrigin.Begin);

                    this.logger.Log(
                        MiddlewareLogLevel,
                        $"Request completed with Response: StatusCode: {context.Response.StatusCode}, Response Model: {text}");

                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
        }
    }
}
