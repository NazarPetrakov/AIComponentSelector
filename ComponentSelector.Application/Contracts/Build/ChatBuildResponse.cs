namespace ComponentSelector.Application.Contracts.Build;

public class ChatBuildResponse
{
    public ChatBuild? Build { get; set; }
    public double TotalPrice { get; set; }
    public Compatibility? Compatibility { get; set; }
}
public class ChatBuild
{
    public required ComponentWithCharacteristicsDto CPU { get; set; }
    public required ComponentWithCharacteristicsDto Motherboard { get; set; }
    public required ComponentWithCharacteristicsDto RAM { get; set; }
    public required ComponentWithCharacteristicsDto Storage { get; set; }
    public required ComponentWithCharacteristicsDto GPU { get; set; }
    public required ComponentWithCharacteristicsDto PSU { get; set; }
    public required ComponentWithCharacteristicsDto Case { get; set; }
}
public class Compatibility
{
    public string? CPU { get; set; }
    public string? Motherboard { get; set; }
    public string? RAM { get; set; }
    public string? GPU { get; set; }
    public string? PSU { get; set; }
    public string? Case { get; set; }
}
