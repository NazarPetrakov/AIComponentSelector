namespace ComponentSelector.Application.Contracts;

public record ComponentWithScoreDto
{
    public required SimpleComponentDto SimpleComponent { get; init; }
    public int Score { get; init; }
}
