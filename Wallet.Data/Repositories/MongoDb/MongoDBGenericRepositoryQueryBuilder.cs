using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Wallet.Domain.Repositories;

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

    public IReadOnlyList<T> ToList()
    {
        return IAsyncCursorSourceExtensions.ToList(this.mongoQueryable);
    }

    public async Task<T?> FirstOrDefaultAsync()
    {
        return await this.mongoQueryable.FirstOrDefaultAsync();
    }

    public T? FirstOrDefault()
    {
        return IAsyncCursorSourceExtensions.FirstOrDefault(this.mongoQueryable);
    }

    public async Task<int> CountAsync()
    {
        return await this.mongoQueryable.CountAsync();
    }

    public async Task<long> LongCountAsync()
    {
        return await this.mongoQueryable.LongCountAsync();
    }
        
    public int Sum(Expression<Func<T, int>> selector)
    {
        return this.mongoQueryable.Sum(selector);
    }

    public async Task<int> SumAsync(Expression<Func<T, int>> selector)
    {
        return await this.mongoQueryable.SumAsync(selector);
    }

    public decimal Sum(Expression<Func<T, decimal>> selector)
    {
        return this.mongoQueryable.Sum(selector);
    }

    public async Task<decimal> SumAsync(Expression<Func<T, decimal>> selector)
    {
        return await this.mongoQueryable.SumAsync(selector);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await this.mongoQueryable.AnyAsync(predicate);
    }

    public bool Any(Expression<Func<T, bool>> predicate)
    {
        return this.mongoQueryable.Any(predicate);
    }

    public IQueryBuilder<T> Where(Expression<Func<T, bool>> predicate)
    {
        this.mongoQueryable = this.mongoQueryable.Where(predicate);
        return this;
    }
        
    public IQueryBuilder<TResult> OfType<TResult>()
        where TResult : notnull
    {
        return new MongoDBGenericRepositoryQueryBuilder<TResult>(this.mongoQueryable.OfType<TResult>());
    }

    public IQueryBuilder<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector)
    {
        this.mongoQueryable = this.mongoQueryable.OrderBy(keySelector);
        return this;
    }

    public IQueryBuilder<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
    {
        this.mongoQueryable = this.mongoQueryable.OrderByDescending(keySelector);
        return this;
    }

    public IQueryBuilder<T> ThenBy<TKey>(Expression<Func<T, TKey>> keySelector)
    {
        if (!(this.mongoQueryable is IOrderedMongoQueryable<T> orderedMongoQueryable))
        {
            throw new InvalidOperationException($"{nameof(this.OrderBy)} or {nameof(this.OrderByDescending)} should be called before {nameof(this.ThenBy)}!");
        }

        this.mongoQueryable = orderedMongoQueryable.ThenBy(keySelector);
        return this;
    }

    public IQueryBuilder<T> ThenByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
    {
        if (!(this.mongoQueryable is IOrderedMongoQueryable<T> orderedMongoQueryable))
        {
            throw new InvalidOperationException($"{nameof(this.OrderBy)} or {nameof(this.OrderByDescending)} should be called before {nameof(this.ThenByDescending)}!");
        }

        this.mongoQueryable = orderedMongoQueryable.ThenByDescending(keySelector);
        return this;
    }

    public IQueryBuilder<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        where TResult : notnull
    {
        return new MongoDBGenericRepositoryQueryBuilder<TResult>(this.mongoQueryable.Select(selector));
    }
        
    public IQueryBuilder<TResult> SelectMany<TResult>(Expression<Func<T, IEnumerable<TResult>>> selector)
        where TResult : notnull
    {
        return new MongoDBGenericRepositoryQueryBuilder<TResult>(this.mongoQueryable.SelectMany(selector));
    }

    public IQueryBuilder<TCustomResult> SelectMany<TResult, TCustomResult>(Expression<Func<T, IEnumerable<TResult>>> collectionSelector, Expression<Func<T, TResult, TCustomResult>> resultSelector)
        where TResult : notnull
        where TCustomResult : notnull
    {
        return new MongoDBGenericRepositoryQueryBuilder<TCustomResult>(this.mongoQueryable.SelectMany(collectionSelector, resultSelector));
    }

    public IQueryBuilder<T> Take(int count)
    {
        this.mongoQueryable = this.mongoQueryable.Take(count);
        return this;
    }

    public IQueryBuilder<T> Skip(int count)
    {
        this.mongoQueryable = this.mongoQueryable.Skip(count);
        return this;
    }

    public IQueryBuilder<T> Distinct()
    {
        this.mongoQueryable = this.mongoQueryable.Distinct();
        return this;
    }

    public override string? ToString()
    {
        return this.mongoQueryable.ToString();
    }

    public IQueryBuilder<IGrouping<TKey, T>> GroupBy<TKey>(Expression<Func<T, TKey>> keySelector)
    {
        return new MongoDBGenericRepositoryQueryBuilder<IGrouping<TKey, T>>(this.mongoQueryable.GroupBy(keySelector));
    }
        
    public IQueryBuilder<TResult> GroupBy<TKey, TResult>(Expression<Func<T, TKey>> keySelector, Expression<Func<TKey, IEnumerable<T>, TResult>> resultSelector)
        where TResult : notnull
    {
        return new MongoDBGenericRepositoryQueryBuilder<TResult>(this.mongoQueryable.GroupBy(keySelector, resultSelector));
    }
}