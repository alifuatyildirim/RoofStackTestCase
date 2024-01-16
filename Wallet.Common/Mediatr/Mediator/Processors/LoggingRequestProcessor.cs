using MediatR;
using Microsoft.Extensions.Logging;

namespace Wallet.Common.Mediatr.Mediator.Processors
{
    public class LoggingRequestProcessor<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingRequestProcessor<TRequest, TResponse>> logger;
        
        public LoggingRequestProcessor(ILogger<LoggingRequestProcessor<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                this.logger.LogInformation($"Handling {typeof(TResponse).FullName}");
                TResponse response = await next().ConfigureAwait(false);
                this.logger.LogInformation($"Handled {typeof(TResponse).FullName}");
                return response;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Handling failed {typeof(TResponse).FullName}");
                throw;
            }
        }
    }
}
