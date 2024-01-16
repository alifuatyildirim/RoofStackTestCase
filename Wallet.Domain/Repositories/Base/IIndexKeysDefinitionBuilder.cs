using System.Linq.Expressions;

namespace Wallet.Domain.Repositories.Base;

public interface IIndexKeysDefinitionBuilder<TEntity>
{
    IIndexKeysDefinition<TEntity> Ascending(Expression<Func<TEntity, object>> field);

    IIndexKeysDefinition<TEntity> Ascending(string field);

    IIndexKeysDefinition<TEntity> Descending(Expression<Func<TEntity, object>> field);

    IIndexKeysDefinition<TEntity> Descending(string field);

    IIndexKeysDefinition<TEntity> Combine(IEnumerable<IIndexKeysDefinition<TEntity>> keys);

    IIndexKeysDefinition<TEntity> Combine(params IIndexKeysDefinition<TEntity>[] keys);
}