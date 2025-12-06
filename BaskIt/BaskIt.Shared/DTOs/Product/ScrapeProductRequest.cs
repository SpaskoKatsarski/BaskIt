using System.ComponentModel.DataAnnotations;

namespace BaskIt.Shared.DTOs.Product;

public class ScrapeProductRequest
{
    [Required]
    [Url]
    public required string PageUrl { get; set; }
}
