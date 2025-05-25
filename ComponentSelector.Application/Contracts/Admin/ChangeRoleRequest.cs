namespace ComponentSelector.Application.Contracts.Admin;

public class ChangeRoleRequest
{
    public string UserName { get; set; } = string.Empty;
    public string NewRole { get; set; } = "User";
}
