using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Application.IServices;
using ComponentSelector.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComponentSelector.API.Controllers;

public class ComponentsController(IComponentsService componentsService) : BaseApiController
{
    [HttpGet]
    public async Task<ICollection<ComponentDto>> GetComponents(
        [FromQuery] ComponentQueryParams queryParams) =>
            await componentsService.GetComponentsAsync(Response, queryParams);
    [HttpGet("{id}")]
    public async Task<ComponentWithCharacteristicsDto> GetComponentById(int id) =>
        await componentsService.GetComponentByIdAsync(id);
    [HttpGet("categories")]
    public IActionResult GetCategories() =>
         Ok(Enum.GetNames(typeof(CategoryEnum)));
    [Authorize]
    [HttpGet("top/{limit}")]
    public async Task<List<ComponentWithScoreDto>> GetTopComponents(
        [FromQuery] CategoryEnum category, [FromQuery] string searchTitle, int limit) =>
            await componentsService.FindTopComponentsWithScoreAsync(category, searchTitle, limit);
    [HttpPost("test")]
    public async Task<ComponentDto?> Test(
        [FromQuery] CategoryEnum category, [FromBody] List<CharacteristicDto> characteristics)
    {
        return await componentsService.FindComponentByCharacteristicsAsync(category, characteristics);
    }
}
