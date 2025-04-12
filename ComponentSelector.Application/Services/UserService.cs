using ComponentSelector.Application.Contracts.User;
using ComponentSelector.Application.IRepositories;
using ComponentSelector.Application.IServices;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace ComponentSelector.Application.Services;

public class UserService(UserManager<AppUser> userManager, IUsersRepository usersRepository) : IUserService
{
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
}
