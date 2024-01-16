using FluentValidation;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging; 

namespace Wallet.Common.Mediatr.Mediator.Processors
{
    public class ValidationRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
        where TRequest : notnull
    {
        private readonly ILogger<ValidationRequestPreProcessor<TRequest>> logger;
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationRequestPreProcessor(ILogger<ValidationRequestPreProcessor<TRequest>> logger, IEnumerable<IValidator<TRequest>> validators)
        {
            this.logger = logger;
            this.validators = validators;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<object>(request);

            List<FluentValidation.Results.ValidationFailure> failures = this.validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count > 0)
            {
                string message = string.Join(", ", failures.Select(x => x.ErrorMessage));
                throw new Exception(message);
            }

            return Task.CompletedTask;
        }
    }
}
