using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Application.IServices;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(AppUser user);
}
