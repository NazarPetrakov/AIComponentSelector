using System.ComponentModel.DataAnnotations;

namespace ComponentSelector.Application.Contracts.User;

public class UpdateUserDto
{
    [Required]
    [StringLength(64, MinimumLength = 1, ErrorMessage = "Username must be between 1 and 64 characters.")]
    public required string UserName { get; set; }

    [Required]
    [Range(1, 120, ErrorMessage = "Age must be between 1 and 120.")]
    public int Age { get; set; }

    [StringLength(256, ErrorMessage = "Country must be less than 256 characters.")]
    public string? Country { get; set; }
}
