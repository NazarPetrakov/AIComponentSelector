using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using AutoMapper;
using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.IServices;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace ComponentSelector.Application.Services;

public class AuthService(UserManager<AppUser> userManager,
    ITokenService tokenService, IMapper mapper) : IAuthService
{
    public async Task<AuthUserDto> LoginAsync(LoginDto loginDto)
    {
        var username = loginDto.UserName 
            ?? throw new InvalidCredentialException("Username is required for login.");

        var user = await userManager.FindByNameAsync(username)
            ?? throw new InvalidCredentialException($"User '{loginDto.UserName}' not found.");

        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result)
            throw new InvalidCredentialException("Incorrect password.");

        return new AuthUserDto
        {
            UserName = username,
            Token = await tokenService.GenerateTokenAsync(user)
        };
    }

    public async Task<AuthUserDto> RegisterAsync(RegisterDto registerDto)
    {
        if (await userManager.FindByNameAsync(registerDto.UserName) != null)
            throw new ConflictException($"User '{registerDto.UserName}' is already registered.");

        var user = mapper.Map<AppUser>(registerDto);
        if (user == null || user.UserName == null)
            throw new ValidationException("Username is required for registration.");

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            var errorMessages = string.Join(";", result.Errors.Select(e => e.Description));
            throw new IdentityException(errorMessages);
        }

        return new AuthUserDto
        {
            UserName = user.UserName,
            Token = await tokenService.GenerateTokenAsync(user)
        };

    }
}
