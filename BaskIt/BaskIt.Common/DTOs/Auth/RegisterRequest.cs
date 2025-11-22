using System.ComponentModel.DataAnnotations;

namespace BaskIt.Shared.DTOs.Auth;
public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 8)]
    public required string Password { get; set; }

    [Required]
    [Compare(nameof(Password))]
    public required string ConfirmPassword { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}
