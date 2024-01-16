using Wallet.Domain.Repositories;

namespace Wallet.Data.Repositories.MongoDb;

public class MongoDBTestTransactionScope : ITransactionScope
{
    private bool isDisposed;

    public void BeginTransaction()
    {
        // Method intentionally left empty.
    }

    public Task CommitTransactionAsync()
    {
        return Task.CompletedTask;
        // Method intentionally left empty.
    }

    public void CommitTransaction()
    {
        // Method intentionally left empty.
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (this.isDisposed)
        {
            return;
        }

        this.isDisposed = true;
    }
}