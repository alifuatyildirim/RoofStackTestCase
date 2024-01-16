using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;
using Wallet.Domain.Repositories;

namespace Wallet.Data.Repositories.MongoDb;

public class MongoDBDatabaseConnection : IDatabaseConnection
{
    public MongoDBDatabaseConnection(IOptions<MongoDBOptions> options)
    {
        MongoClientSettings clientSettings = MongoClientSettings.FromConnectionString(options.Value.ConnectionString);
        if (options.Value.ActivateSubscriber ?? false)
        {
            clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());
        }
      
        this.Client = new MongoClient(clientSettings);
        this.Database = this.Client.GetDatabase(options.Value.Database);
    }

    internal MongoClient Client { get; }

    internal IMongoDatabase Database { get; }

    public virtual async Task<ITransactionScope> BeginTransactionScopeAsync()
    {
        IClientSessionHandle session = await this.Client.StartSessionAsync();
        return new MongoDBTransactionScope(this, session);
    }

    public virtual ITransactionScope BeginTransactionScope()
    {
        IClientSessionHandle session = this.Client.StartSession();
        return new MongoDBTransactionScope(this, session);
    }
}