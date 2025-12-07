using BaskIt.Commands.Basket.AddProduct;
using BaskIt.Commands.Basket.Create;
using BaskIt.Commands.Basket.Delete;
using BaskIt.Commands.Basket.GetBasketById;
using BaskIt.Commands.Basket.GetUserBaskets;
using BaskIt.Commands.Basket.RemoveProduct;
using BaskIt.Commands.Basket.Update;
using BaskIt.Shared.DTOs.Basket;
using BaskIt.Shared.DTOs.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BaskIt.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IMediator mediator;
    private string? UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public BasketController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBasket([FromBody] CreateBasketRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = new CreateBasketCommand(UserId!, request);
        var basketId = await mediator.Send(command);

        return Ok(basketId);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BasketListDto>))]
    public async Task<IActionResult> GetUserBaskets()
    {
        var query = new GetUserBasketsQuery(UserId!);
        var baskets = await mediator.Send(query);

        return Ok(baskets);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDetailsDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBasketById(string id)
    {
        var query = new GetBasketByIdQuery(id);
        var basket = await mediator.Send(query);

        if (basket == null)
        {
            return NotFound($"Basket with ID {id} not found.");
        }

        return Ok(basket);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBasket(string id, [FromBody] UpdateBasketRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = new UpdateBasketCommand(id, request);
        await mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBasket(string id)
    {
        var command = new DeleteBasketCommand(id);
        await mediator.Send(command);

        return NoContent();
    }

    [HttpPost("{id}/products")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddProductToBasket(string id, [FromBody] ProductScrapedDto product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = new AddProductToBasketCommand(id, product);
        await mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{basketId}/products/{productId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveProductFromBasket(string basketId, string productId)
    {
        var command = new RemoveProductFromBasketCommand(basketId, productId);
        await mediator.Send(command);

        return NoContent();
    }
}
