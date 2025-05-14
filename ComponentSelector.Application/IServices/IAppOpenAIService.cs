using ComponentSelector.Application.Contracts.Build;
using ComponentSelector.Application.Contracts.OpenAi;

namespace ComponentSelector.Application.IServices;

public interface IAppOpenAIService
{
    Task<ChatBuildResponse> GetChatBuildAsync(ChatBuildRequest request);
    Task<string> AskBotAsync(string userId, string userMessage);
    Task<List<BotMessageDto>> GetUserChatMessages(string userId);
    Task ClearChatHistory(string userId);
}
