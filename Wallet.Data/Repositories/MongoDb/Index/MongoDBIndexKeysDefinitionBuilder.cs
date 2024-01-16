using System.Linq.Expressions;
using Wallet.Domain.Repositories.Base;

namespace Wallet.Data.Repositories.MongoDb.Index;

public sealed class MongoDBIndexKeysDefinitionBuilder<TEntity> : IIndexKeysDefinitionBuilder<TEntity>
{
    private readonly global::MongoDB.Driver.IndexKeysDefinitionBuilder<TEntity> builder = new global::MongoDB.Driver.IndexKeysDefinitionBuilder<TEntity>();

    public IIndexKeysDefinition<TEntity> Ascending(Expression<Func<TEntity, object>> field)
    {
        return new MongoDBIndexKeysDefinition<TEntity>(this.builder.Ascending(field));
    }
    
    public IIndexKeysDefinition<TEntity> Ascending(string field)
    {
        return new MongoDBIndexKeysDefinition<TEntity>(this.builder.Ascending(field));
    }

    public IIndexKeysDefinition<TEntity> Descending(Expression<Func<TEntity, object>> field)
    {
        return new MongoDBIndexKeysDefinition<TEntity>(this.builder.Descending(field));
    }
    
    public IIndexKeysDefinition<TEntity> Descending(string field)
    {
        return new MongoDBIndexKeysDefinition<TEntity>(this.builder.Descending(field));
    }

    public IIndexKeysDefinition<TEntity> Combine(IEnumerable<IIndexKeysDefinition<TEntity>> keys)
    {
        return new MongoDBIndexKeysDefinition<TEntity>(this.builder.Combine(keys.OfType<MongoDBIndexKeysDefinition<TEntity>>().Select(key => key.IndexKeysDefinition)));
    }

    public IIndexKeysDefinition<TEntity> Combine(params IIndexKeysDefinition<TEntity>[] keys)
    {
        return this.Combine((IEnumerable<IIndexKeysDefinition<TEntity>>)keys);
    }
}