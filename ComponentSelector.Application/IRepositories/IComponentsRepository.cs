using ComponentSelector.Application.Helpers.Pagination;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Specification;

namespace ComponentSelector.Application.IRepositories;

public interface IComponentsRepository
{
    Task<PagedList<Component>> GetComponentsAsync(ComponentQueryParams componentQueryParams,
        BaseSpecification<Component> baseSpecification);
}
