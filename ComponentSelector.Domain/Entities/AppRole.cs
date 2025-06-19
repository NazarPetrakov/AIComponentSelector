using Microsoft.AspNetCore.Identity;

namespace ComponentSelector.Domain.Entities;

public class AppRole : IdentityRole<int>
{
     public ICollection<AppUserRole> UserRoles { get; set; } = [];
}
