using System.ComponentModel.DataAnnotations;

namespace ComponentSelector.Application.Contracts.User;

public class ChangePasswordDto
{
    [Required]
    [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 16 characters.")]
    public required string CurrentPassword { get; set; }
    [Required]
    [StringLength(16, MinimumLength = 8, ErrorMessage = "New password must be between 8 and 16 characters.")]
    public required string NewPassword { get; set; }
}
