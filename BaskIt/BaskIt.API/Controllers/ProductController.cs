using BaskIt.Commands.Product.Add;
using BaskIt.Shared.DTOs.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaskIt.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator mediator;

    public ProductController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("scrape")]
    [ProducesResponseType(typeof(ProductScrapedDto), StatusCodes.Status200OK, Type = typeof(ProductScrapedDto))]
    [ProducesResponseType(typeof(ProductScrapedDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Scrape([FromBody] ScrapeProductRequest request)
    {
        var command = new ProductScrapeCommand(request.PageUrl);

        var result = await this.mediator.Send(command);

        return Ok(result);
    }
}
