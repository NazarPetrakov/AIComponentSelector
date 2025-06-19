namespace ComponentSelector.Application.Contracts;

public class ErrorResponse
{
    public string? Title { get; set; }
    public int Status { get; set; }
    public string? Message { get; set; }
    public string? Details { get; set; }

}
