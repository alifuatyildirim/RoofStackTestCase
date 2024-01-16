using System.Linq.Expressions;

namespace Wallet.Domain.Repositories.Base;

public interface IQueryBuilder<T>
{
    Task<IReadOnlyList<T>> ToListAsync();
    IQueryBuilder<T> Where(Expression<Func<T, bool>> predicate);
}
