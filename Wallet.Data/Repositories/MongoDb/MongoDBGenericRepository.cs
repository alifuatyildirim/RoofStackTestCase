using System.Diagnostics.CodeAnalysis; 
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

    public async Task<ITransactionScope> BeginTransactionScopeAsync()
    {
        return await this.databaseConnection.BeginTransactionScopeAsync();
    }
    public async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await this.Collection.AsQueryable().Where(c => c.Id.Equals(id)).FirstOrDefaultAsync();
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
}