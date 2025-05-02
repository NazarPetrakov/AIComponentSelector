using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Contracts.Build;
using ComponentSelector.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComponentSelector.API.Controllers;
[Authorize]
public class BuildsController(IBuildService buildService) : BaseApiController
{
    [HttpPost]
    public async Task<BuildDto> CreateBuild(CreateBuildDto createBuildDto) =>
        await buildService.BuildPC(createBuildDto);
    [HttpGet]
    public async Task<BuildDto> GetAiBuildByPrice([FromQuery] ChatBuildRequest request) =>
            await buildService.AIBuildPC(request);
}
