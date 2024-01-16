using MongoDB.Driver;
using Wallet.Domain.Repositories;

namespace Wallet.Data.Repositories.MongoDb;

public class MongoDBTransactionScope : ITransactionScope
{
    private readonly MongoDBDatabaseConnection databaseConnection;
    private bool isDisposed;

    public MongoDBTransactionScope(MongoDBDatabaseConnection databaseConnection, IClientSessionHandle session)
    {
        this.databaseConnection = databaseConnection;
        this.Session = session;
    }

    public IClientSessionHandle Session { get; }

    public void BeginTransaction()
    {
        this.Session.StartTransaction();
    }

    public async Task CommitTransactionAsync()
    {
        await this.Session.CommitTransactionAsync();
    }

    public void CommitTransaction()
    {
        this.Session.CommitTransaction();
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

        if (disposing)
        {
            if (this.Session.IsInTransaction)
            {
                this.Session.AbortTransaction();
            }

            this.Session.Dispose();
        }

        this.isDisposed = true;
    }
}