using System.Linq.Expressions;
using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Domain.Specification;

public static class PredicateBuilder
{
    public static Expression<Func<TEntity, bool>> And<TEntity>(
        this Expression<Func<TEntity, bool>> first,
        Expression<Func<TEntity, bool>> second) where TEntity : BaseEntity
    {
        var parameter = Expression.Parameter(typeof(TEntity));
        var body = Expression.AndAlso(
            Expression.Invoke(first, parameter),
            Expression.Invoke(second, parameter));
        return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
    }
}
