using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ComponentSelector.API.Controllers;

public class ComponentsController(IComponentsService componentsService) : BaseApiController
{
    [HttpGet]
    public async Task<ICollection<ComponentDto>> GetComponents([FromQuery] ComponentQueryParams queryParams) =>
        await componentsService.GetComponentsAsync(Response, queryParams);
}
