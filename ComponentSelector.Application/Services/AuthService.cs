using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using AutoMapper;
using ComponentSelector.Application.Contracts.User;
using ComponentSelector.Application.IServices;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace ComponentSelector.Application.Services;

public class AuthService(UserManager<AppUser> userManager,
    ITokenService tokenService, IMapper mapper) : IAuthService
{
    public async Task ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto)
    {
        var appUser = await userManager.FindByIdAsync(userId)
            ?? throw new ItemNotFoundException($"No user with id: {userId}");

        var passwordResult = await userManager.ChangePasswordAsync(appUser,
            changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

        if (!passwordResult.Succeeded)
        {
            throw new IdentityException(passwordResult);
        }
        await userManager.UpdateAsync(appUser);
    }
    public async Task ChangeEmailAsync(string userId, ChangeEmailDto changeEmailDto)
    {
        var appUser = await userManager.FindByIdAsync(userId)
            ?? throw new ItemNotFoundException($"No user with id: {userId}");

        var emailResult = await userManager.SetEmailAsync(appUser, changeEmailDto.NewEmail);

        if (!emailResult.Succeeded)
        {
            throw new IdentityException(emailResult);
        }
        await userManager.UpdateAsync(appUser);
    }
    public async Task<UserDto> GetCurrentUserAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException("No user Id.");

        var user = await userManager.FindByIdAsync(userId) ??
            throw new ArgumentNullException($"No user with id: {userId}.");

        var userRoles = await userManager.GetRolesAsync(user);
        var role = userRoles.FirstOrDefault();

        var userDto = mapper.Map<UserDto>(user);

        if (!string.IsNullOrEmpty(role))
            userDto.Role = role;

        return userDto;
    }
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
            throw new IdentityException(result);
        }

        await userManager.AddToRoleAsync(user, "User");

        return new AuthUserDto
        {
            UserName = user.UserName,
            Token = await tokenService.GenerateTokenAsync(user)
        };

    }
}
