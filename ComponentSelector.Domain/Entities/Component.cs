using System.Collections.ObjectModel;

namespace ComponentSelector.Domain.Entities;

public class Component : BaseEntity
{
    public string? Category { get; set; }
    public string? Title { get; set; }
    public double? Price { get; set; }
    public string? Availability { get; set; }
    public string? Link { get; set; }
    public string? ImageUrl { get; set; }
    public int? Reviews { get; set; }

    public List<Characteristic> Characteristics{ get; set; } = [];

}
