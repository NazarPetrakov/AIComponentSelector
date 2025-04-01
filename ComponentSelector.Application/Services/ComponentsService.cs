using ComponentSelector.Application.IRepositories;
using ComponentSelector.Application.IServices;
using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Application.Services;

public class ComponentsService(IComponentsRepository componentsRepository) : IComponentsService
{
    public async Task<ICollection<Component>> GetComponentsAsync()
    {
        return await componentsRepository.GetComponentsAsync();
    }
}
