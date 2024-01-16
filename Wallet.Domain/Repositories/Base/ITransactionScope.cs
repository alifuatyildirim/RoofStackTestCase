namespace Wallet.Domain.Repositories;

public interface ITransactionScope : IDisposable
{
    void BeginTransaction();

    void CommitTransaction();

    Task CommitTransactionAsync();
}