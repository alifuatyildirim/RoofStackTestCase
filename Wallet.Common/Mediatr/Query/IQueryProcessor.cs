namespace Wallet.Common.Mediatr.Query
{
    public interface IQueryProcessor
    {
        Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query);
    }
}
