using System.ComponentModel.DataAnnotations;

namespace BaskIt.API.Models;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }

    public bool RememberMe { get; set; }
}
