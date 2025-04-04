using System.Text;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ComponentSelector.API.Extensions;

public static class AuthenticationExtension
{
    public static IServiceCollection AddAuthServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<AppUser>(o =>
        {
            o.Password.RequiredLength = 8;
            o.Password.RequireNonAlphanumeric = false;

        }).AddRoles<AppRole>().AddRoleManager<RoleManager<AppRole>>()
            .AddEntityFrameworkStores<AppDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var key = configuration.GetSection("JwtToken").GetValue<string>("key")
                    ?? throw new InvalidOperationException("Can not get token key from configuration");

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });
        return services;
    }
}
