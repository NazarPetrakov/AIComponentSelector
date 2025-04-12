using ComponentSelector.Application.Contracts.User;

namespace ComponentSelector.Application.IServices;

public interface IUserService
{
    Task UpdateUserAsync(string userId, UpdateUserDto user);
}
