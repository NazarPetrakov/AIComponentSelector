using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Helpers.Pagination;
using ComponentSelector.Application.Helpers.QueryParams;
using Microsoft.AspNetCore.Http;

namespace ComponentSelector.Application.IServices;

public interface IComponentsService
{
    Task<PagedList<ComponentDto>> GetComponentsAsync(
        HttpResponse httpResponse, ComponentQueryParams componentQueryParams);
}
