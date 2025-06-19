using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Application.Contracts;

public class CharacteristicDto : BaseEntity
{
    public required string AttributeName { get; set; }
    public required string AttributeValue { get; set;}

    public int ComponentId { get; set; }
}
