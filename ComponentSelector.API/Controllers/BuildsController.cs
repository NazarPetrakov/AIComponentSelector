using ComponentSelector.API.Extensions;
using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Contracts.Build;
using ComponentSelector.Application.Helpers.Pagination;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Application.IServices;
using ComponentSelector.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComponentSelector.API.Controllers;

[Authorize]
public class BuildsController(IBuildService buildService) : BaseApiController
{
    // [HttpPost("")]
    // public async Task<BuildDto> BuildPC(SearchBuildDto createBuildDto) =>
    //     await buildService.BuildPC(createBuildDto);
    [HttpGet("ai-generate")]
    public async Task<BuildDto> GetAiBuildByPrice([FromQuery] ChatBuildRequest request) =>
        await buildService.AIBuildPC(request);
    [HttpPost]
    public async Task<int> CreateUserBuild(CreateUserBuildDto createUserBuild) =>
        await buildService.CreateBuildAsync(createUserBuild, User.GetUserId());
    [HttpGet("{id}")]
    public async Task<UserBuildDto> GetBuildById(int id) =>
        await buildService.GetBuildByIdAsync(id);
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("users/{id}")]
    public async Task<PagedList<UserBuildDto>> GetBuildsByUserId([FromQuery] BuildQueryParams buildQueryParams,
        string id) =>
            await buildService.GetPagedUserBuildsAsync(id, buildQueryParams, Response);
    [HttpGet("users/me")]
    public async Task<PagedList<UserBuildDto>> GetCurrentUserBuilds([FromQuery] BuildQueryParams buildQueryParams) =>
        await buildService.GetPagedUserBuildsAsync(User.GetUserId(), buildQueryParams, Response);
    [HttpDelete("{id}")]
    public async Task DeleteBuildAsync(int id) => await buildService.DeleteBuildAsync(id, User.GetUserId());
}
