using ComponentSelector.Application.Helpers.Pagination;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Application.IRepositories;

public interface IComponentsRepository
{
    Task<PagedList<Component>> GetComponentsAsync(ComponentQueryParams componentQueryParams);
}
