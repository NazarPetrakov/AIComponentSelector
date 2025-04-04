using ComponentSelector.Application.Contracts;

namespace ComponentSelector.Application.IServices;

public interface IAuthService
{
    Task<AuthUserDto> LoginAsync(LoginDto loginDto);
    Task<AuthUserDto> RegisterAsync(RegisterDto registerDto);
    
}
