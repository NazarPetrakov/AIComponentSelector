using AutoMapper;
using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Extensions;
using ComponentSelector.Application.Helpers.Pagination;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Application.IRepositories;
using ComponentSelector.Application.IServices;
using Microsoft.AspNetCore.Http;

namespace ComponentSelector.Application.Services;

public class ComponentsService(IComponentsRepository componentsRepository, IMapper mapper) : IComponentsService
{
    public async Task<PagedList<ComponentDto>> GetComponentsAsync(
        HttpResponse httpResponse, ComponentQueryParams componentQueryParams)
    {
        var components = await componentsRepository.GetComponentsAsync(componentQueryParams);

        httpResponse.AddPaginationHeader(components);

        var componentDtos = mapper.Map<List<ComponentDto>>(components);

        return new PagedList<ComponentDto>
        (
            componentDtos,
            components.TotalCount,
            components.CurrentPage,
            components.PageSize
        );
    }
}
