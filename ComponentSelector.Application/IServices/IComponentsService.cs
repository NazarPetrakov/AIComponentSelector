using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Application.IServices;

public interface IComponentsService
{
    Task<ICollection<Component>> GetComponentsAsync();
}
