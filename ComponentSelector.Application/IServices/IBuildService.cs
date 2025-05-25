using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Contracts.Build;
using ComponentSelector.Application.Helpers.Pagination;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Specification;
using Microsoft.AspNetCore.Http;

namespace ComponentSelector.Application.IServices;

public interface IBuildService
{
    Task<BuildDto> BuildPC(SearchBuildDto createBuildDto);
    Task<BuildDto> AIBuildPC(ChatBuildRequest request);
    Task<PagedList<UserBuildDto>> GetPagedBuildsAsync(BuildQueryParams buildQueryParams,
        HttpResponse httpResponse, BaseSpecification<Build> spec);
    Task<UserBuildDto> GetBuildByIdAsync(int id);
    Task<PagedList<UserBuildDto>> GetPagedUserBuildsAsync(string userId,
        BuildQueryParams buildQueryParams, HttpResponse httpResponse);
    Task<int> CreateBuildAsync(CreateUserBuildDto build, string userId);
    Task DeleteBuildAsync(int buildId, string userId);
}
