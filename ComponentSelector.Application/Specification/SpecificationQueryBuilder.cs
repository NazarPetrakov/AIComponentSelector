using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Specification;
using Microsoft.EntityFrameworkCore;

namespace ComponentSelector.Application.Specification;

public static class SpecificationQueryBuilder
{
    public static IQueryable<TEntity> GetQuery<TEntity>(IQueryable<TEntity> inputQuery,
        BaseSpecification<TEntity> specification) where TEntity : BaseEntity
    {
        var query = inputQuery.AsNoTracking();

        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }
        if (specification.Includes != null)
        {
            query = specification.Includes.Aggregate(query, (current, include)
                => current.Include(include));
        }
        if (specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }
        if (specification.OrderByDesc != null)
        {
            query = query.OrderByDescending(specification.OrderByDesc);
        }

        return query;
    }
}
