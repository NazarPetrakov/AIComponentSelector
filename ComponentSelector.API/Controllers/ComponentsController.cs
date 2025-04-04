using ComponentSelector.Application.IServices;
using ComponentSelector.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ComponentSelector.API.Controllers;

public class ComponentsController(IComponentsService componentsService) : BaseApiController
{
    [HttpGet]
    public async Task<ICollection<Component>> GetComponents() =>
        await componentsService.GetComponentsAsync();
}
