namespace ComponentSelector.Application.Helpers.QueryParams;

public class ComponentQueryParams : PaginationQueryParams
{
    public string? Category { get; set; }
    public string OrderBy { get; set; } = "id";
    public string? OrderByDesc { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public bool? Availability { get; set; }
}
