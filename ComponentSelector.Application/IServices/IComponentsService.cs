using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Helpers.Pagination;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace ComponentSelector.Application.IServices;

public interface IComponentsService
{
    Task<PagedList<ComponentDto>> GetComponentsAsync
        (HttpResponse httpResponse, ComponentQueryParams componentQueryParams);
    Task<List<ComponentWithScoreDto>> FindTopComponentsWithScoreAsync
        (CategoryEnum category, string searchName, int limit);
    Task<ComponentWithCharacteristicsDto> GetComponentByIdAsync(int id);
    Task<ComponentDto> FindComponentByTitleAsync(CategoryEnum category, string title);
}
