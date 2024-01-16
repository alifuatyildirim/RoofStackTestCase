using MediatR;
using Wallet.Common.Mediatr.Command;
using Wallet.Common.Mediatr.Query;

namespace Wallet.Common.Mediatr.Mediator
{
    public class CommandQueryMediator : IApplicationCommandSender, IDomainCommandSender, IQueryProcessor
    {
        private readonly IMediator mediator;

        public CommandQueryMediator(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query)
        {
            return this.mediator.Send(query);
        }

        public Task SendAsync(IApplicationCommand command)
        {
            return this.mediator.Send(command);
        }

        public Task<TResult> SendAsync<TResult>(IApplicationCommand<TResult> command)
        {
            return this.mediator.Send(command);
        }
        
        public Task SendAsync(IDomainCommand command)
        {
            return this.mediator.Send(command);
        }
    }
}
