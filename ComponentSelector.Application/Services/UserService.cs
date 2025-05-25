using ComponentSelector.Application.Contracts.Admin;
using ComponentSelector.Application.Contracts.User;
using ComponentSelector.Application.IRepositories;
using ComponentSelector.Application.IServices;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ComponentSelector.Application.Services;

public class UserService(UserManager<AppUser> userManager, IUsersRepository usersRepository) : IUserService
{
    public async Task DeleteChatThreadIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId)
            ?? throw new ItemNotFoundException("User not found");

        user.ChatThreadId = null;
        await userManager.UpdateAsync(user);
    }

    public async Task<string?> GetChatThreadIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId)
            ?? throw new ItemNotFoundException("User not found");

        return user?.ChatThreadId;
    }

    public async Task<AppUser> GetUserByIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId)
            ?? throw new ItemNotFoundException($"User with id: {userId} not found.");

        return user;
    }

    public async Task UpdateChatThreadIdAsync(string userId, string threadId)
    {
        var user = await userManager.FindByIdAsync(userId)
            ?? throw new ItemNotFoundException("User not found");

        user.ChatThreadId = threadId;
        await userManager.UpdateAsync(user);
    }

    public async Task UpdateUserAsync(string userId, UpdateUserDto updateUser)
    {
        var appUser = await userManager.FindByIdAsync(userId)
            ?? throw new ItemNotFoundException($"No user with id: {userId}");

        var setUsernameResult = await userManager.SetUserNameAsync(appUser, updateUser.UserName);
        if (!setUsernameResult.Succeeded)
            throw new IdentityException(setUsernameResult);

        appUser.Age = updateUser.Age;
        appUser.Country = updateUser.Country;

        usersRepository.UpdateUser(appUser);

        var isSuccessful = await usersRepository.SaveChangesAsync();
        if (!isSuccessful)
            throw new InvalidOperationException("Failed to save changes to the user.");
    }
    public async Task ChangeUserRoleAsync(ChangeRoleRequest changeRoleRequest)
    {
        var user = await userManager.FindByNameAsync(changeRoleRequest.UserName) ??
            throw new BadHttpRequestException($"No user with username: {changeRoleRequest.UserName}"); ;

        var currentRoles = await userManager.GetRolesAsync(user);
        var removeResult = await userManager.RemoveFromRolesAsync(user, currentRoles);

        if (!removeResult.Succeeded)
            throw new IdentityException(removeResult);

        var addResult = await userManager.AddToRoleAsync(user, changeRoleRequest.NewRole);

        if (!addResult.Succeeded)
            throw new IdentityException(addResult);
    }
}
