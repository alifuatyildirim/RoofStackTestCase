using MongoDB.Driver;
using Wallet.Domain.Repositories.Base;

namespace Wallet.Data.Repositories.MongoDb.Index;

internal class MongoDBIndexKeysDefinition<TEntity> : IIndexKeysDefinition<TEntity>
{
    internal MongoDBIndexKeysDefinition(IndexKeysDefinition<TEntity> indexKeysDefinition)
    {
        this.IndexKeysDefinition = indexKeysDefinition;
    }

    internal IndexKeysDefinition<TEntity> IndexKeysDefinition { get; }
}