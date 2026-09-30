using System.ComponentModel.DataAnnotations;

namespace BankTask.Application.DTOs.Authentication;

public class LoginRequest
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [MaxLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
    public string Password { get; set; } = string.Empty;
}