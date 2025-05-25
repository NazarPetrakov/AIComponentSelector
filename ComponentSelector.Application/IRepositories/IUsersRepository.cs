using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Application.IRepositories;

public interface IUsersRepository
{
    void UpdateUser(AppUser user);
    Task<bool> SaveChangesAsync();
    Task<int> GetTotalUsersAsync();
}
