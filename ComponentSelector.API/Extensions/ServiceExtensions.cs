using ComponentSelector.API.Exceptions;
using ComponentSelector.Application.IRepositories;
using ComponentSelector.Application.IServices;
using ComponentSelector.Application.Services;
using ComponentSelector.Infrastructure.Data;
using ComponentSelector.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ComponentSelector.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddAppServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddCors();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<IComponentsRepository, ComponentsRepository>();
        services.AddScoped<IComponentsService, ComponentsService>();
        services.AddScoped<ICharacteristicsRepository, CharacteristicsRepository>();
        services.AddScoped<ICharacteristicsService, CharacteristicsService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOpenAIService, OpenAIService>();
        services.AddScoped<IBuildService, BuildService>();


        return services;
    }
}
