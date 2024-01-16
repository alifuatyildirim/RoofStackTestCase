using MediatR;

namespace Wallet.Common.Mediatr.Command
{
    public interface IApplicationCommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult>
        where TCommand : IApplicationCommand<TResult>
    {
    }
}
