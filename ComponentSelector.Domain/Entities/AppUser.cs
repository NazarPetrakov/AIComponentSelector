using Microsoft.AspNetCore.Identity;

namespace ComponentSelector.Domain.Entities;

public class AppUser : IdentityUser<int>
{
    public int Age { get; set; }
    public string? Country { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<AppUserRole> UserRoles { get; set; } = [];
}
