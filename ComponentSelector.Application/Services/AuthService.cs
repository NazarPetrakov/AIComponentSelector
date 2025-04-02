using System.Security.Authentication;
using AutoMapper;
using ComponentSelector.Application.DTOs;
using ComponentSelector.Application.IServices;
using ComponentSelector.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ComponentSelector.Application.Services;

public class AuthService(UserManager<AppUser> userManager,
    ITokenService tokenService, IMapper mapper) : IAuthService
{
    public async Task<AuthUserDto> LoginAsync(LoginDto loginDto)
    {
        var username = loginDto.UserName ?? throw new Exception("No username for login");

        var user = await userManager.FindByNameAsync(username)
            ?? throw new Exception("User with this username not found");

        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result)
            throw new AuthenticationException("Wrong password");

        return new AuthUserDto
        {
            UserName = username,
            Token = await tokenService.GenerateTokenAsync(user)
        };
    }

    public async Task<AuthUserDto> RegisterAsync(RegisterDto registerDto)
    {
        if (await userManager.FindByNameAsync(registerDto.UserName) != null)
            throw new ArgumentException("A user with this username is already registered.");

        var user = mapper.Map<AppUser>(registerDto);
        if (user == null || user.UserName == null)
            throw new Exception("Enter the username to register");

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            var errorMessages = string.Join(";\n", result.Errors.Select(e => e.Description));
            throw new Exception($"User registration failed: \n{errorMessages}");
        }

        return new AuthUserDto
        {
            UserName = user.UserName,
            Token = await tokenService.GenerateTokenAsync(user)
        };

    }
}
