using ComponentSelector.Application.Helpers.Pagination;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Application.IRepositories;
using ComponentSelector.Application.Specification;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Specification;
using ComponentSelector.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComponentSelector.Infrastructure.Repositories;

public class ComponentsRepository(AppDbContext context) : IComponentsRepository
{
    public async Task<PagedList<Component>> GetComponentsAsync(
        ComponentQueryParams componentQueryParams, BaseSpecification<Component> spec)
    {
        var query = context.Components.AsNoTracking();

        query = SpecificationQueryBuilder.GetQuery(query, spec);

        return await PagedList<Component>.PaginateAsync(query,
            componentQueryParams.PageNumber, componentQueryParams.PageSize);
    }
}
