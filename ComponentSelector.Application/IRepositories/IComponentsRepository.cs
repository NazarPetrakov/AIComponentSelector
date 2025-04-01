using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Application.IRepositories;

public interface IComponentsRepository
{
    Task<ICollection<Component>> GetComponentsAsync();
}
