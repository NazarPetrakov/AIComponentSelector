using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Application.Contracts;

public class ComponentDto : BaseEntity
{
    public string? Category { get; set; }
    public string? Title { get; set; }
    public double? Price { get; set; }
    public string? Availability { get; set; }
    public string? Link { get; set; }
    public string? ImageUrl { get; set; }
    public string? Reviews { get; set; }

}
