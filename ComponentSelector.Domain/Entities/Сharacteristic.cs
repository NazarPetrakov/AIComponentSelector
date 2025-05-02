namespace ComponentSelector.Domain.Entities;

public class Characteristic : BaseEntity
{
    public required string AttributeName { get; set; }
    public required string AttributeValue { get; set;}

    public int ComponentId { get; set; }
    public Component Component{ get; set; }
}
