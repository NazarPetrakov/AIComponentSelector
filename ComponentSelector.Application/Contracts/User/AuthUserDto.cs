namespace ComponentSelector.Application.Contracts.User;

public class AuthUserDto
{
    public required int Id { get; set; }
    public required string Token { get; set; }
    public required string UserName { get; set; }
}
