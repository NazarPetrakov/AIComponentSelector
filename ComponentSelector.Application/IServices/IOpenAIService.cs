using ComponentSelector.Application.Contracts.Build;

namespace ComponentSelector.Application.IServices;

public interface IOpenAIService
{
    Task<ChatBuildResponse> GetChatBuildAsync(ChatBuildRequest request);
}
