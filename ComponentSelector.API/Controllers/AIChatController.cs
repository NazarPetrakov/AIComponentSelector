using ComponentSelector.API.Extensions;
using ComponentSelector.Application.Contracts.Build;
using ComponentSelector.Application.Contracts.OpenAi;
using ComponentSelector.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComponentSelector.API.Controllers;
[Authorize]
public class AIChatController(IAppOpenAIService openAiService) : BaseApiController
{
    [HttpGet("build")]
    public async Task<ChatBuildResponse> GetChatBuildAsync([FromQuery] ChatBuildRequest request) =>
        await openAiService.GetChatBuildAsync(request);
    [HttpPost("ask")]
    public async Task<string> AskBotAsync(AskBotRequest request) =>
        await openAiService.AskBotAsync(User.GetUserId(), request.Message);
    [HttpGet("messages")]
    public async Task<List<BotMessageDto>> GetMessages() =>
        await openAiService.GetUserChatMessages(User.GetUserId());
    [HttpDelete]
    public async Task ClearChatHistory() =>
        await openAiService.ClearChatHistory(User.GetUserId());
}
