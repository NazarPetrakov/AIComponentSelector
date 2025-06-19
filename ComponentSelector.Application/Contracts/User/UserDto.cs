namespace ComponentSelector.Application.Contracts.User;

public class UserDto
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public int Age { get; set; }
    public string? Country { get; set; }
    public string? Role { get; set; }
    public DateTime CreatedAt { get; set; }
}
