using ComponentSelector.Application.IRepositories;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComponentSelector.Infrastructure.Repositories;

public class UsersRepository(AppDbContext context) : IUsersRepository
{
    public void UpdateUser(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }
    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
