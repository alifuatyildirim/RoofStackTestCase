using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Wallet.Domain.Repositories;
using Wallet.Domain.Repositories.Base;

namespace Wallet.Data.Repositories.MongoDb;

internal class MongoDBGenericRepositoryQueryBuilder<T> : IQueryBuilder<T>
    where T : notnull
{
    private IMongoQueryable<T> mongoQueryable;

    internal MongoDBGenericRepositoryQueryBuilder(IMongoQueryable<T> mongoQueryable)
    {
        this.mongoQueryable = mongoQueryable;
    }

    public async Task<IReadOnlyList<T>> ToListAsync()
    {
        return await this.mongoQueryable.ToListAsync();
    }

    public IQueryBuilder<T> Where(Expression<Func<T, bool>> predicate)
    {
        this.mongoQueryable = this.mongoQueryable.Where(predicate);
        return this;
    }
}