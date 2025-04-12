using ComponentSelector.API.Extensions;
using ComponentSelector.Application.Contracts.User;
using ComponentSelector.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComponentSelector.API.Controllers;

public class AccountController(IAuthService authService, IUserService userService) : BaseApiController
{
    [HttpPost("login")]
    public async Task<AuthUserDto> Login(LoginDto loginDto) =>
        await authService.LoginAsync(loginDto);
    [HttpPost("register")]
    public async Task<AuthUserDto> Register(RegisterDto registerDto) =>
        await authService.RegisterAsync(registerDto);
    [Authorize]
    [HttpGet("me")]
    public async Task<UserDto> GetCurrentUser() =>
        await authService.GetCurrentUserAsync(User.GetUserId());
    [Authorize]
    [HttpPost("change-password")]
    public async Task ChangePassword(ChangePasswordDto changePasswordDto) =>
        await authService.ChangePasswordAsync(User.GetUserId(), changePasswordDto);
    [Authorize]
    [HttpPost("change-email")]
    public async Task ChangeEmail(ChangeEmailDto changeEmailDto) =>
        await authService.ChangeEmailAsync(User.GetUserId(), changeEmailDto);
    [Authorize]
    [HttpPut("update-user")]
    public async Task UpdateUser(UpdateUserDto updateUserDto) =>
        await userService.UpdateUserAsync(User.GetUserId(), updateUserDto);
}
