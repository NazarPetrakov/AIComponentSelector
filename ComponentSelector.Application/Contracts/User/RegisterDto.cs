using System.ComponentModel.DataAnnotations;

namespace ComponentSelector.Application.Contracts.User;

public class RegisterDto
{
    [Required]
    [StringLength(64, MinimumLength = 1, ErrorMessage = "Username must be between 1 and 64 characters.")]
    public required string UserName { get; set; }

    [Required]
    [StringLength(256, MinimumLength = 1, ErrorMessage = "Email must be between 1 and 256 characters.")]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public required string Email { get; set; }

    [Required]
    [Range(1, 120, ErrorMessage = "Age must be between 1 and 120.")]
    public int Age { get; set; }
    [StringLength(256, ErrorMessage = "Country must be less than 256 characters.")]
    public string? Country { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 16 characters.")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    [StringLength(16, MinimumLength = 8, ErrorMessage = "Confirm password must be between 8 and 16 characters.")]
    public required string ConfirmedPassword { get; set; }
}
