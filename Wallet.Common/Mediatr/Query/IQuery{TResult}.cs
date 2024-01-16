using MediatR;

namespace Wallet.Common.Mediatr.Query
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
