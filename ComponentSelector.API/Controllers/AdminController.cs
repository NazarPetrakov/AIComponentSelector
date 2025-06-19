using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Contracts.Admin;
using ComponentSelector.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComponentSelector.API.Controllers;

[Authorize(Policy = "RequireAdminRole")]
public class AdminController(IAdminService adminService, IUserService userService) : BaseApiController
{
    [HttpGet("stats")]
    public async Task<StatisticsDto> GetStatistics() => await adminService.GetStatisticsAsync();
    [HttpPost("change-role")]
    public async Task ChangeRole(ChangeRoleRequest changeRoleRequest) =>
        await userService.ChangeUserRoleAsync(changeRoleRequest);
}
