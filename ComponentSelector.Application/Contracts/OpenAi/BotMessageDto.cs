namespace ComponentSelector.Application.Contracts.OpenAi;

public class BotMessageDto
{
    public required string Message { get; set; }
    public required string Role { get; set; }
}
