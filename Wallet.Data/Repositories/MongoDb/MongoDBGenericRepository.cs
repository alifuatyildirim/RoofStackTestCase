using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Pluralize.NET; 
using Wallet.Data.Repositories.MongoDb.Index;
using Wallet.Domain.Entities.BaseEntity;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.Base;

namespace Wallet.Data.Repositories.MongoDb;

public sealed class MongoDBGenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>
    where TId : IEquatable<TId>
{
    private readonly IPluralize pluralizer;
    private readonly MongoDBDatabaseConnection databaseConnection;
    private readonly Lazy<IMongoCollection<TEntity>> lazyCollection;
    private string? customCollectionName;

    public MongoDBGenericRepository(
        IDatabaseConnection databaseConnection,
        IIndexKeysDefinitionBuilder<TEntity> indexKeysDefinitionBuilder,
        IPluralize pluralizer)
    {
        if (!(databaseConnection is MongoDBDatabaseConnection mongoDBDatabaseConnection))
        {
            throw new InvalidOperationException("IDatabaseConnection is not MongoDBDatabaseConnection!");
        }

        if (!(indexKeysDefinitionBuilder is MongoDBIndexKeysDefinitionBuilder<TEntity>))
        {
            throw new InvalidOperationException("IIndexKeysDefinitionBuilder is not MongoDBIndexKeysDefinitionBuilder!");
        }

        this.pluralizer = pluralizer;
        this.databaseConnection = mongoDBDatabaseConnection;
        this.lazyCollection = new Lazy<IMongoCollection<TEntity>>(() => this.CreateCollectionFactory());
        this.IndexKeys = indexKeysDefinitionBuilder;
    }

    public string? CustomCollectionName
    {
        get => this.customCollectionName;

        set
        {
            if (this.lazyCollection.IsValueCreated)
            {
                throw new InvalidOperationException("Cannot set the CustomCollectionName after the collections is created!");
            }

            this.customCollectionName = value;
        }
    }

    private string DefaultCollectionName
    {
        get
        {
            string plural = this.pluralizer.Pluralize(typeof(TEntity).Name);
            return plural.Substring(0, 1).ToLowerInvariant() + plural.Substring(1);
        }
    }

    private IMongoCollection<TEntity> Collection
    {
        get { return this.lazyCollection.Value; }
    }

    public bool CollectionExists
    {
        get
        {
            var filter = new BsonDocument("name", this.GetCollectionName());
            var options = new ListCollectionNamesOptions { Filter = filter };

            return this.databaseConnection.Database.ListCollectionNames(options).Any();
        }
    }

    public IIndexKeysDefinitionBuilder<TEntity> IndexKeys { get; } 
    public bool CreateCollection()
    {
        if (this.CollectionExists)
        { 
            return false;
        }

        this.databaseConnection.Database.CreateCollection(this.Collection.CollectionNamespace.CollectionName);
        return true;
    } 
    public bool DropCollection()
    {
        if (!this.CollectionExists)
        {
            return false;
        }

        this.databaseConnection.Database.DropCollection(this.Collection.CollectionNamespace.CollectionName);
        return true;
    }

    public async Task<ITransactionScope> BeginTransactionScopeAsync()
    {
        return await this.databaseConnection.BeginTransactionScopeAsync();
    }

    public ITransactionScope BeginTransactionScope()
    {
        return this.databaseConnection.BeginTransactionScope();
    }

    public async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await this.Collection.AsQueryable().Where(c => c.Id.Equals(id)).FirstOrDefaultAsync();
    }

    public TEntity? GetById(TId id)
    {
        return IAsyncCursorSourceExtensions.FirstOrDefault(this.Collection.AsQueryable().Where(c => c.Id.Equals(id)));
    }

    public async Task<bool> ExistsByIdAsync(TId id)
    {
        int count = await this.Collection.AsQueryable().Where(c => c.Id.Equals(id)).CountAsync();
        return count > 0;
    }

    public bool ExistsById(TId id)
    {
        int count = Queryable.Count(this.Collection.AsQueryable().Where(c => c.Id.Equals(id)));
        return count > 0;
    }

    public IQueryBuilder<TEntity> Query()
    {
        return new MongoDBGenericRepositoryQueryBuilder<TEntity>(this.Collection.AsQueryable());
    }

    public async Task AddOneAsync(TEntity entity, ITransactionScope? transactionScope = null)
    {
        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            await this.Collection.InsertOneAsync(session, entity);
        }
        else
        {
            await this.Collection.InsertOneAsync(entity);
        }
    }

    public void AddOne(TEntity entity, ITransactionScope? transactionScope = null)
    {
        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            this.Collection.InsertOne(session, entity);
        }
        else
        {
            this.Collection.InsertOne(entity);
        }
    }

    public async Task AddManyAsync(ICollection<TEntity> entities, ITransactionScope? transactionScope = null)
    {
        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            await this.Collection.InsertManyAsync(session, entities);
        }
        else
        {
            await this.Collection.InsertManyAsync(entities);
        }
    }

    public void AddMany(ICollection<TEntity> entities, ITransactionScope? transactionScope = null)
    {
        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            this.Collection.InsertMany(session, entities);
        }
        else
        {
            this.Collection.InsertMany(entities);
        }
    }

    public async Task UpdateAsync(TEntity entity, ITransactionScope? transactionScope = null)
    {
        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            await this.Collection.ReplaceOneAsync(session, e => e.Id.Equals(entity.Id), entity);
        }
        else
        {
            await this.Collection.ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity);
        }
    }

    public async Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> filter, ITransactionScope? transactionScope = null)
    {
        Expression<Func<TEntity, bool>> idFilter = e => e.Id.Equals(entity.Id);
        FilterDefinition<TEntity> combinedFilter = filter & (FilterDefinition<TEntity>)idFilter;

        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            await this.Collection.ReplaceOneAsync(session, combinedFilter, entity);
        }
        else
        {
            await this.Collection.ReplaceOneAsync(combinedFilter, entity);
        }
    }

    public void Update(TEntity entity, ITransactionScope? transactionScope = null)
    {
        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            this.Collection.ReplaceOne(session, e => e.Id.Equals(entity.Id), entity);
        }
        else
        {
            this.Collection.ReplaceOne(e => e.Id.Equals(entity.Id), entity);
        }
    }

    public void Update(TEntity entity, Expression<Func<TEntity, bool>> filter, ITransactionScope? transactionScope = null)
    {
        Expression<Func<TEntity, bool>> idFilter = e => e.Id.Equals(entity.Id);
        FilterDefinition<TEntity> combinedFilter = filter & (FilterDefinition<TEntity>)idFilter;

        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            this.Collection.ReplaceOne(session, combinedFilter, entity);
        }
        else
        {
            this.Collection.ReplaceOne(combinedFilter, entity);
        }
    }

    public async Task UpsertAsync(TEntity entity, ITransactionScope? transactionScope = null)
    {
        var replaceOptions = new ReplaceOptions() { IsUpsert = true };

        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            await this.Collection.ReplaceOneAsync(session, e => e.Id.Equals(entity.Id), entity, replaceOptions);
        }
        else
        {
            await this.Collection.ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity, replaceOptions);
        }
    }

    public void Upsert(TEntity entity, ITransactionScope? transactionScope = null)
    {
        var replaceOptions = new ReplaceOptions() { IsUpsert = true };

        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            this.Collection.ReplaceOne(session, e => e.Id.Equals(entity.Id), entity, replaceOptions);
        }
        else
        {
            this.Collection.ReplaceOne(e => e.Id.Equals(entity.Id), entity, replaceOptions);
        }
    }

    public async Task DeleteAsync(TId id, ITransactionScope? transactionScope = null)
    {
        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            await this.Collection.DeleteOneAsync(session, e => e.Id.Equals(id));
        }
        else
        {
            await this.Collection.DeleteOneAsync(e => e.Id.Equals(id));
        }
    }

    public void Delete(TId id, ITransactionScope? transactionScope = null)
    {
        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            this.Collection.DeleteOne(session, e => e.Id.Equals(id));
        }
        else
        {
            this.Collection.DeleteOne(e => e.Id.Equals(id));
        }
    }

    public async Task DeleteManyAsync(ICollection<TId> ids, ITransactionScope? transactionScope = null)
    {
        await this.DeleteManyAsync(e => ids.Contains(e.Id), transactionScope);
    }

    public async Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter, ITransactionScope? transactionScope = null)
    {
        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            await this.Collection.DeleteManyAsync(session, filter);
        }
        else
        {
            await this.Collection.DeleteManyAsync(filter);
        }
    }

    public void DeleteMany(ICollection<TId> ids, ITransactionScope? transactionScope = null)
    {
        this.DeleteMany(e => ids.Contains(e.Id), transactionScope);
    }

    public void DeleteMany(Expression<Func<TEntity, bool>> filter, ITransactionScope? transactionScope = null)
    {
        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            this.Collection.DeleteMany(session, filter);
        }
        else
        {
            this.Collection.DeleteMany(filter);
        }
    }

    public async Task UpdateManyAsync<TField>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TField>> field,
        TField value,
        ITransactionScope? transactionScope = null)
    {
        UpdateDefinition<TEntity> updateDefinition = new UpdateDefinitionBuilder<TEntity>().Set(field, value);

        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            await this.Collection.UpdateManyAsync(session, filter, updateDefinition);
        }
        else
        {
            await this.Collection.UpdateManyAsync(filter, updateDefinition);
        }
    }

    public async Task UpdateManyAsync<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, ITransactionScope? transactionScope = null)
    {
        // TODO: su anda coklu update icin butun fieldlerin ayni tipte olmasi gerekiyor, ya da TField olarak object kullanmak
        // bunun yerine farklı bir update yapisi koymak lazim, boylece projector generic repository kullanabilir
        var builder = new UpdateDefinitionBuilder<TEntity>();

        UpdateDefinition<TEntity> updateDefinition = builder.Combine(updates.Select(update => builder.Set(update.Key, update.Value)));

        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            await this.Collection.UpdateManyAsync(session, filter, updateDefinition);
        }
        else
        {
            await this.Collection.UpdateManyAsync(filter, updateDefinition);
        }
    }

    public async Task<TEntity> FindOneAndUpdateAsync<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, Expression<Func<TEntity, int>>? increment = null, ITransactionScope? transactionScope = null)
    {
        return await this.LocalFindOneAndUpdateAsync(filter, updates, isUpsert: false, increment, transactionScope);
    }

    public TEntity FindOneAndUpdate<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, Expression<Func<TEntity, int>>? increment = null, ITransactionScope? transactionScope = null)
    {
        return this.LocalFindOneAndUpdate(filter, updates, isUpsert: false, increment, transactionScope);
    }

    public async Task<TEntity> FindOneAndUpsertAsync<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, Expression<Func<TEntity, int>>? increment = null, ITransactionScope? transactionScope = null)
    {
        return await this.LocalFindOneAndUpdateAsync(filter, updates, isUpsert: true, increment, transactionScope);
    }

    public TEntity FindOneAndUpsert<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, Expression<Func<TEntity, int>>? increment = null, ITransactionScope? transactionScope = null)
    {
        return this.LocalFindOneAndUpdate(filter, updates, isUpsert: true, increment, transactionScope);
    }

    public void UpdateMany<TField>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TField>> field,
        TField value,
        ITransactionScope? transactionScope = null)
    {
        UpdateDefinition<TEntity> updateDefinition = new UpdateDefinitionBuilder<TEntity>().Set(field, value);

        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            this.Collection.UpdateMany(session, filter, updateDefinition);
        }
        else
        {
            this.Collection.UpdateMany(filter, updateDefinition);
        }
    }

    public void UpdateMany<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, ITransactionScope? transactionScope = null)
    { 
        var builder = new UpdateDefinitionBuilder<TEntity>();

        UpdateDefinition<TEntity> updateDefinition = builder.Combine(updates.Select(update => builder.Set(update.Key, update.Value)));

        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            this.Collection.UpdateMany(session, filter, updateDefinition);
        }
        else
        {
            this.Collection.UpdateMany(filter, updateDefinition);
        }
    }

    private IMongoCollection<TEntity> CreateCollectionFactory()
    {
        return this.databaseConnection.Database.GetCollection<TEntity>(this.GetCollectionName());
    }

    private string GetCollectionName()
    {
        return this.CustomCollectionName ?? this.DefaultCollectionName;
    }

    private static bool TryGetCurrentSession(ITransactionScope? transactionScope, [NotNullWhen(true)] out IClientSessionHandle? session)
    {
        if (transactionScope is MongoDBTransactionScope mongoDBTransactionScope)
        {
            session = mongoDBTransactionScope.Session;
            return true;
        }

        session = null;
        return false;
    }

    public async Task UpsertOneByFilterAsync<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, ITransactionScope? transactionScope = null)
    {
        var builder = new UpdateDefinitionBuilder<TEntity>();

        UpdateDefinition<TEntity> updateDefinition = builder.Combine(updates.Select(update => builder.Set(update.Key, update.Value)));
        var updateOptions = new UpdateOptions() { IsUpsert = true };

        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            await this.Collection.UpdateOneAsync(session, filter, updateDefinition, options: updateOptions);
        }
        else
        {
            await this.Collection.UpdateOneAsync(filter, updateDefinition, options: updateOptions);
        }
    }
        
    public void UpsertOneByFilter<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, ITransactionScope? transactionScope = null)
    {
        var builder = new UpdateDefinitionBuilder<TEntity>();

        UpdateDefinition<TEntity> updateDefinition = builder.Combine(updates.Select(update => builder.Set(update.Key, update.Value)));
        var updateOptions = new UpdateOptions() { IsUpsert = true };

        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            this.Collection.UpdateOne(session, filter, updateDefinition, options: updateOptions);
        }
        else
        {
            this.Collection.UpdateOne(filter, updateDefinition, options: updateOptions);
        }
    }

    private async Task<TEntity> LocalFindOneAndUpdateAsync<TField>(
        Expression<Func<TEntity, bool>> filter,
        IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates,
        bool isUpsert,
        Expression<Func<TEntity, int>>? increment = null,
        ITransactionScope? transactionScope = null)
    {
        FilterDefinition<TEntity> filterDefinition = FilterDefinition(filter, updates, isUpsert, increment, out UpdateDefinition<TEntity> updateDefinition, out FindOneAndUpdateOptions<TEntity>? options);

        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            return await this.Collection.FindOneAndUpdateAsync(
                session,
                filterDefinition,
                updateDefinition,
                options);
        }

        return await this.Collection.FindOneAndUpdateAsync(filterDefinition, updateDefinition, options);
    }
        
    private TEntity LocalFindOneAndUpdate<TField>(
        Expression<Func<TEntity, bool>> filter,
        IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates,
        bool isUpsert,
        Expression<Func<TEntity, int>>? increment = null,
        ITransactionScope? transactionScope = null)
    {
        FilterDefinition<TEntity> filterDefinition = FilterDefinition(filter, updates, isUpsert, increment, out UpdateDefinition<TEntity> updateDefinition, out FindOneAndUpdateOptions<TEntity>? options);

        if (TryGetCurrentSession(transactionScope, out IClientSessionHandle? session))
        {
            return this.Collection.FindOneAndUpdate(
                session,
                filterDefinition,
                updateDefinition,
                options);
        }

        return this.Collection.FindOneAndUpdate(filterDefinition, updateDefinition, options);
    }

    private static FilterDefinition<TEntity> FilterDefinition<TField>(
        Expression<Func<TEntity, bool>> filter, 
        IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, 
        bool isUpsert,
        Expression<Func<TEntity, int>>? increment, 
        out UpdateDefinition<TEntity> updateDefinition, 
        out FindOneAndUpdateOptions<TEntity>? options)
    {
        FilterDefinition<TEntity> filterDefinition = Builders<TEntity>.Filter.Where(filter);

        var builder = new UpdateDefinitionBuilder<TEntity>();
        updateDefinition = builder.Combine();

        if (increment != null)
        {
            updateDefinition = updateDefinition.Inc(increment, 1);
        }

        foreach (KeyValuePair<Expression<Func<TEntity, TField>>, TField> field in updates)
        {
            updateDefinition = updateDefinition.Set(field.Key, field.Value);
        }

        options = new FindOneAndUpdateOptions<TEntity>
        {
            IsUpsert = isUpsert,
            ReturnDocument = ReturnDocument.After,
        };
            
        return filterDefinition;
    }

    public string GenerateStringId()
    {
        return ObjectId.GenerateNewId().ToString();
    }
}