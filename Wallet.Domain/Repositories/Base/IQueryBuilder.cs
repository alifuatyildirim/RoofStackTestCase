using System.Linq.Expressions;

namespace Wallet.Domain.Repositories;

public interface IQueryBuilder<T>
{
    IReadOnlyList<T> ToList();

    Task<IReadOnlyList<T>> ToListAsync();

    Task<T?> FirstOrDefaultAsync();

    T? FirstOrDefault();

    Task<int> CountAsync();

    Task<long> LongCountAsync();

    int Sum(Expression<Func<T, int>> selector);

    Task<int> SumAsync(Expression<Func<T, int>> selector);

    decimal Sum(Expression<Func<T, decimal>> selector);

    Task<decimal> SumAsync(Expression<Func<T, decimal>> selector);

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    bool Any(Expression<Func<T, bool>> predicate);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "natural name")]
    IQueryBuilder<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        where TResult : notnull;

    IQueryBuilder<TResult> SelectMany<TResult>(Expression<Func<T, IEnumerable<TResult>>> selector)
        where TResult : notnull;

    IQueryBuilder<TCustomResult> SelectMany<TResult, TCustomResult>(
        Expression<Func<T, IEnumerable<TResult>>> collectionSelector,
        Expression<Func<T, TResult, TCustomResult>> resultSelector)
        where TResult : notnull
        where TCustomResult : notnull;

    IQueryBuilder<T> Where(Expression<Func<T, bool>> predicate);

    IQueryBuilder<TResult> OfType<TResult>()
        where TResult : notnull;

    IQueryBuilder<IGrouping<TKey, T>> GroupBy<TKey>(Expression<Func<T, TKey>> keySelector);

    IQueryBuilder<TResult> GroupBy<TKey, TResult>(Expression<Func<T, TKey>> keySelector, Expression<Func<TKey, IEnumerable<T>, TResult>> resultSelector)
        where TResult : notnull;

    IQueryBuilder<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector);

    IQueryBuilder<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector);

    IQueryBuilder<T> ThenBy<TKey>(Expression<Func<T, TKey>> keySelector);

    IQueryBuilder<T> ThenByDescending<TKey>(Expression<Func<T, TKey>> keySelector);

    IQueryBuilder<T> Take(int count);

    IQueryBuilder<T> Skip(int count);

    IQueryBuilder<T> Distinct();
}
