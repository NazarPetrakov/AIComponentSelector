using ComponentSelector.Application.Contracts.Build;

namespace ComponentSelector.Application.Contracts;

public class BuildDto
{
    public required ComponentDto CPU { get; set; }
    public required ComponentDto Motherboard { get; set; }
    public required ComponentDto RAM { get; set; }
    public required ComponentDto Storage { get; set; }
    public required ComponentDto GPU { get; set; }
    public required ComponentDto PSU { get; set; }
    public required ComponentDto Case { get; set; }

    public double TotalPrice { get; set; }

    public Compatibility? Compatibility { get; set; }

}
