using System.Text.Json;
using ComponentSelector.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ComponentSelector.Infrastructure.Data;

public static class Seed
{
    public static async Task SeedUsersAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        if (await userManager.Users.AnyAsync()) return;

        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string path = Path.Combine(basePath, "Data", "DataSeed.json");

        var file = await File.ReadAllTextAsync(path);

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var users = JsonSerializer.Deserialize<List<AppUser>>(file, options);

        if (users == null) return;

        var roles = new List<AppRole>
        {
            new() {Name = "User"},
            new() {Name = "Admin"},
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        foreach (var item in users)
        {
            await userManager.CreateAsync(item, "Pa$$w0rd");
            await userManager.AddToRoleAsync(item, "User");
        }

        var admin = new AppUser
        {
            UserName = "Admin",
            Age = 99,
        };

        await userManager.CreateAsync(admin, "Pa$$w0rd");
        await userManager.AddToRoleAsync(admin, "Admin");

    }
}
