using ComponentSelector.Application.Contracts.Admin;
using ComponentSelector.Application.Contracts.User;
using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Application.IServices;

public interface IUserService
{
    Task UpdateUserAsync(string userId, UpdateUserDto user);
    Task<string?> GetChatThreadIdAsync(string userId);
    Task UpdateChatThreadIdAsync(string userId, string threadId);
    Task DeleteChatThreadIdAsync(string userId);
    Task<AppUser> GetUserByIdAsync(string userId);
    Task ChangeUserRoleAsync(ChangeRoleRequest changeRoleRequest);
}
