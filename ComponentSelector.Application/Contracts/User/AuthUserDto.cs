namespace ComponentSelector.Application.Contracts.User;

public class AuthUserDto
{
    public required string Token { get; set; }
    public required string UserName { get; set; }
}
