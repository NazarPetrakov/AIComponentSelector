using ComponentSelector.Application.DTOs;

namespace ComponentSelector.Application.IServices;

public interface IAuthService
{
    Task<AuthUserDto> LoginAsync(LoginDto loginDto);
    Task<AuthUserDto> RegisterAsync(RegisterDto registerDto);
    
}
