using ComponentSelector.Application.Contracts.Build;
using ComponentSelector.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComponentSelector.API.Controllers;
[Authorize]
public class AIChatsController(IOpenAIService openAiService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<ChatBuildResponse>> GetChatBuildAsync([FromQuery] ChatBuildRequest request)
    {
        return Ok(await openAiService.GetChatBuildAsync(request));
    }
}
