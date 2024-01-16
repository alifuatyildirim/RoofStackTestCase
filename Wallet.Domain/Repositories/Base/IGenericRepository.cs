using System.Linq.Expressions;
using Wallet.Domain.Entities.BaseEntity;

namespace Wallet.Domain.Repositories.Base;

public interface IGenericRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>
    where TId : IEquatable<TId>
{
    string? CustomCollectionName { get; set; }

    bool CollectionExists { get; } 
 
    bool CreateCollection(); 
    bool DropCollection();
 

    IQueryBuilder<TEntity> Query(); 

    TEntity? GetById(TId id);

    Task<TEntity> GetByIdAsync(TId id);

    bool ExistsById(TId id);

    Task<bool> ExistsByIdAsync(TId id);

    Task<ITransactionScope> BeginTransactionScopeAsync();

    ITransactionScope BeginTransactionScope();

    Task AddOneAsync(TEntity entity, ITransactionScope? transactionScope = null);

    void AddOne(TEntity entity, ITransactionScope? transactionScope = null);

    void AddMany(ICollection<TEntity> entities, ITransactionScope? transactionScope = null);

    Task AddManyAsync(ICollection<TEntity> entities, ITransactionScope? transactionScope = null);

    Task UpdateAsync(TEntity entity, ITransactionScope? transactionScope = null);

    Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> filter, ITransactionScope? transactionScope = null);

    void Update(TEntity entity, ITransactionScope? transactionScope = null);

    void Update(TEntity entity, Expression<Func<TEntity, bool>> filter, ITransactionScope? transactionScope = null);

    Task UpsertAsync(TEntity entity, ITransactionScope? transactionScope = null);

    void Upsert(TEntity entity, ITransactionScope? transactionScope = null);

    Task DeleteAsync(TId id, ITransactionScope? transactionScope = null);

    void Delete(TId id, ITransactionScope? transactionScope = null);

    void DeleteMany(ICollection<TId> ids, ITransactionScope? transactionScope = null);

    void DeleteMany(Expression<Func<TEntity, bool>> filter, ITransactionScope? transactionScope = null);

    Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter, ITransactionScope? transactionScope = null);

    Task DeleteManyAsync(ICollection<TId> ids, ITransactionScope? transactionScope = null);

    void UpdateMany<TField>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TField>> field,
        TField value,
        ITransactionScope? transactionScope = null);

    void UpdateMany<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, ITransactionScope? transactionScope = null);

    Task UpdateManyAsync<TField>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TField>> field,
        TField value,
        ITransactionScope? transactionScope = null);

    Task UpdateManyAsync<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, ITransactionScope? transactionScope = null);

    Task<TEntity> FindOneAndUpdateAsync<TField>(
        Expression<Func<TEntity, bool>> filter,
        IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates,
        Expression<Func<TEntity, int>>? increment = null,
        ITransactionScope? transactionScope = null);

    TEntity FindOneAndUpdate<TField>(
        Expression<Func<TEntity, bool>> filter,
        IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates,
        Expression<Func<TEntity, int>>? increment = null,
        ITransactionScope? transactionScope = null);
        
    Task<TEntity> FindOneAndUpsertAsync<TField>(
        Expression<Func<TEntity, bool>> filter,
        IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates,
        Expression<Func<TEntity, int>>? increment = null,
        ITransactionScope? transactionScope = null);

    TEntity FindOneAndUpsert<TField>(
        Expression<Func<TEntity, bool>> filter,
        IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates,
        Expression<Func<TEntity, int>>? increment = null,
        ITransactionScope? transactionScope = null);
        
    Task UpsertOneByFilterAsync<TField>(
        Expression<Func<TEntity, bool>> filter,
        IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates,
        ITransactionScope? transactionScope = null);

    void UpsertOneByFilter<TField>(
        Expression<Func<TEntity, bool>> filter,
        IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates,
        ITransactionScope? transactionScope = null);
        
    string GenerateStringId();
}