using System.ComponentModel.DataAnnotations;

namespace BaskIt.API.Models;

public class UserLogin
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}
