using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using ComponentSelector.Application.IServices;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ComponentSelector.Application.Services;

public class TokenService(UserManager<AppUser> userManager,
    IConfiguration configuration) : ITokenService
{
    public async Task<string> GenerateTokenAsync(AppUser user)
    {
        var key = configuration.GetSection("JwtToken").GetValue<string>("key")
            ?? throw new InvalidConfigurationException("JWT token key is missing in configuration.");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        if (user.UserName == null)
            throw new InvalidUserException("User must have a valid username to generate a token.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName),
        };

        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
}
