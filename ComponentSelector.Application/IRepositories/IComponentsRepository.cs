using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Helpers.Pagination;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Specification;

namespace ComponentSelector.Application.IRepositories;

public interface IComponentsRepository
{
    Task<PagedList<Component>> GetPagedComponentsAsync(ComponentQueryParams componentQueryParams,
        BaseSpecification<Component> baseSpecification);
    Task<List<SimpleComponentDto>> GetSimpleComponentsAsync(
        BaseSpecification<Component> spec);

    Task<List<string>> GetComponentTitlesAsync(BaseSpecification<Component> spec);
    Task<Component?> GetComponentByIdAsync(int id, BaseSpecification<Component>? spec);
    IQueryable<Component> GetComponentsQuery(BaseSpecification<Component> spec);
}
