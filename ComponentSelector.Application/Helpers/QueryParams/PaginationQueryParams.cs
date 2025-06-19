namespace ComponentSelector.Application.Helpers.QueryParams;

public class PaginationQueryParams
{
    const int maxPageSize = 100;
    private int _pageSize = 20;
    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get { return _pageSize; }
        set
        {
            if (value > maxPageSize) _pageSize = maxPageSize;
            _pageSize = value;
        }
    }
}
