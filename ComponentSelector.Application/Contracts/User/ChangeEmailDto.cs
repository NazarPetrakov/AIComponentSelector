using System.ComponentModel.DataAnnotations;

namespace ComponentSelector.Application.Contracts.User;

public class ChangeEmailDto
{
    [Required]
    [StringLength(256, MinimumLength = 1, ErrorMessage = "Email must be between 1 and 256 characters.")]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public required string NewEmail { get; set; }
}
