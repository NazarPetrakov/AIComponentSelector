using ComponentSelector.Application.Contracts;
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
    public async Task<Component?> GetComponentByIdAsync(int id, BaseSpecification<Component>? spec)
    {
        var query = context.Components.AsNoTracking();

        if (spec != null)
            query = SpecificationQueryBuilder.GetQuery(query, spec);

        return await query.FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task<PagedList<Component>> GetPagedComponentsAsync(
        ComponentQueryParams componentQueryParams, BaseSpecification<Component> spec)
    {
        var query = context.Components.AsNoTracking();

        query = SpecificationQueryBuilder.GetQuery(query, spec);

        return await PagedList<Component>.PaginateAsync(query,
            componentQueryParams.PageNumber, componentQueryParams.PageSize);
    }
    public IQueryable<Component> GetComponentsQuery(BaseSpecification<Component> spec)
    {
        var query = context.Components.AsNoTracking();

        return SpecificationQueryBuilder.GetQuery(query, spec);
    }
    public async Task<List<SimpleComponentDto>> GetSimpleComponentsAsync(
        BaseSpecification<Component> spec)
    {
        var componentsQuery = context.Components.AsNoTracking();

        var components = SpecificationQueryBuilder.GetQuery(componentsQuery, spec)
            .Select(c => new SimpleComponentDto(c.Id, c.Title ?? ""));

        return await components.ToListAsync();
    }
    public async Task<List<string>> GetComponentTitlesAsync(BaseSpecification<Component> spec)
    {
        var componentsQuery = context.Components.AsNoTracking();

        return await SpecificationQueryBuilder.GetQuery(componentsQuery, spec)
            .Select(c => c.Title ?? "")
            .ToListAsync();
    }
    public async Task<int> GetTotalComponentsAsync()
    {
        return await context.Components.CountAsync();
    }
}
