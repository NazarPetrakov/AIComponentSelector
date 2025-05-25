using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Application.Contracts;

public class ComponentWithCharacteristicsDto : BaseEntity
{
    public string? Category { get; set; }
    public string? Title { get; set; }
    public double? Price { get; set; }
    public string? Availability { get; set; }
    public string? Link { get; set; }
    public string? ImageUrl { get; set; }
    public int? Reviews { get; set; }
    
    public List<CharacteristicDto> Characteristics { get; set; } = [];
}
