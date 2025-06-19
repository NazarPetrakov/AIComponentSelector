
using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Application.Contracts.Build;

public class UserBuildDto
{
    public int Id { get; set; }
    public ComponentDto? CPU { get; set; }

    public ComponentDto? Motherboard { get; set; }

    public ComponentDto? RAM { get; set; }

    public ComponentDto? Storage { get; set; }

    public ComponentDto? GPU { get; set; }

    public ComponentDto? PSU { get; set; }

    public ComponentDto? Case { get; set; }

    public int UserId { get; set; }

    public double TotalPrice { get; set; }

}
