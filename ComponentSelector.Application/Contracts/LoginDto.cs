using System.ComponentModel.DataAnnotations;

namespace ComponentSelector.Application.Contracts;

public class LoginDto
{
    [Required]
    [Length(1, 64, ErrorMessage = "Username must be between 1 and 64 characters.")]
    public required string UserName { get; set; }

    [Required]
    [Length(8, 16, ErrorMessage = "Password must be between 8 and 16 characters.")]
    public required string Password { get; set; }
}
