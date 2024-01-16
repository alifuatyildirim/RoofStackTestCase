using Wallet.Domain.Entities.BaseEntity;

namespace Wallet.Domain.Repositories.Base;

public interface IGenericRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>
    where TId : IEquatable<TId>
{ 
 
    bool CreateCollection();   

    IQueryBuilder<TEntity> Query();

    Task<TEntity> GetByIdAsync(TId id);

    Task<ITransactionScope> BeginTransactionScopeAsync();

    Task AddOneAsync(TEntity entity, ITransactionScope? transactionScope = null);

    Task UpdateAsync(TEntity entity, ITransactionScope? transactionScope = null);
}