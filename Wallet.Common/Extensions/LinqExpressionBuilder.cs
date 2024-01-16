using System.Linq.Expressions;
using LinqKit;

namespace Wallet.Common.Extensions
{
    public static class LinqExpressionBuilder
    {
        public static Expression<Func<T, bool>> New<T>(bool defaultExpression)
        {
            return PredicateBuilder.New<T>(defaultExpression);
        }

        public static Expression<Func<T, bool>> New<T>(Expression<Func<T, bool>>? expr = null)
        {
            return PredicateBuilder.New<T>(expr);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            return expr1.Extend(expr2, PredicateOperator.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            return expr1.Extend(expr2, PredicateOperator.Or);
        }
        
        public static Expression<Func<T, bool>> ExpressionNot<T>(this Expression<Func<T, bool>> expr)
        {
            return expr.Not();
        }
    }
}
