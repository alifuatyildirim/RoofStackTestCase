using MediatR;

namespace Wallet.Common.Mediatr.Command
{
    public interface IDomainCommandHandler<in TCommand> : IRequestHandler<TCommand>
        where TCommand : IDomainCommand
    {
    }
}
