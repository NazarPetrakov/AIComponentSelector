using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Application.Contracts;

public class SimpleComponentDto : BaseEntity
{
    public string? Title { get; set; }

    public SimpleComponentDto(int id, string title)
    {
        Id = id;
        Title = title;
    }
}
