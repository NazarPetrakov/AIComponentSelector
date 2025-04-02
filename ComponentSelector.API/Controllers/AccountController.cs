using ComponentSelector.Application.DTOs;
using ComponentSelector.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ComponentSelector.API.Controllers;

public class AccountController(IAuthService authService) : BaseApiController
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthUserDto>> Login(LoginDto loginDto)
    {
        return Ok(await authService.LoginAsync(loginDto));
    }
    [HttpPost("register")]
    public async Task<ActionResult<AuthUserDto>> Register(RegisterDto registerDto)
    {
        return Ok(await authService.RegisterAsync(registerDto));
    }

}
