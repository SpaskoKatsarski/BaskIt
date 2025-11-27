using System.ComponentModel.DataAnnotations;

namespace BaskIt.Shared.DTOs.Product;

public class AddProductRequest
{
    [Required]
    [Url]
    public required string PageUrl { get; set; }
}
