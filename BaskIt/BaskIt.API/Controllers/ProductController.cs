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
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddProductRequest request)
    {
        var command = new ProductAddCommand(request.PageUrl);
        // TODO: Send user id in order to add the product to the correct user's basket

        var result = await this.mediator.Send(command);

        return Ok(result);
    }
}
