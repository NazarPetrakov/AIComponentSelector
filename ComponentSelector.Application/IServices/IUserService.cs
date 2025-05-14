using ComponentSelector.Application.Contracts.User;

namespace ComponentSelector.Application.IServices;

public interface IUserService
{
    Task UpdateUserAsync(string userId, UpdateUserDto user);
    Task<string?> GetChatThreadIdAsync(string userId);
    Task UpdateChatThreadIdAsync(string userId, string threadId);
    Task DeleteChatThreadIdAsync(string userId);
}
