using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ComponentSelector.API.Controllers;

public class AccountController(IAuthService authService) : BaseApiController
{
    [HttpPost("login")]
    public async Task<AuthUserDto> Login(LoginDto loginDto) =>
        await authService.LoginAsync(loginDto);
    [HttpPost("register")]
    public async Task<AuthUserDto> Register(RegisterDto registerDto) =>
        await authService.RegisterAsync(registerDto);

}
