using ComponentSelector.Application.Contracts.User;

namespace ComponentSelector.Application.IServices;

public interface IAuthService
{
    Task<AuthUserDto> LoginAsync(LoginDto loginDto);
    Task<AuthUserDto> RegisterAsync(RegisterDto registerDto);
    Task<UserDto> GetCurrentUserAsync(string userId);
    Task ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
    Task ChangeEmailAsync(string userId, ChangeEmailDto changeEmailDto);
    Task DeleteUserAsync(string userId);
}
