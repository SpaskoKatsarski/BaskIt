using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaskIt.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IMediator mediator;

    public BasketController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBasket()
    {
        // TODO: Implement basket creation
        return Ok();
    }
}
