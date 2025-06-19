using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComponentSelector.API.Controllers;

public class CharacteristicsController(ICharacteristicsService characteristicsService) : BaseApiController
{
    [Authorize]
    [HttpGet("components/{id}")]
    public async Task<ICollection<CharacteristicDto>> GetCharacteristicsByComponentId(
        [FromQuery] CharacteristicsQueryParams queryParams, int id) =>
            await characteristicsService.GetCharacteristicsByComponentIdAsync(id, queryParams);
}
