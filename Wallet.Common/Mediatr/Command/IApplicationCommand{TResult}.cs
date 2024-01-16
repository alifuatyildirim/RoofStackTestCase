using MediatR;

namespace Wallet.Common.Mediatr.Command
{
    public interface IApplicationCommand<out TResult> : IRequest<TResult>
    {
    }
}
