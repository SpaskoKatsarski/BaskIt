using BaskIt.Commands.Login;
using BaskIt.Commands.Register;
using BaskIt.Shared.DTOs.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BaskIt.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator mediator;

    public AuthController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = await this.mediator.Send(new RegisterCommand(request));

        return Ok(token);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = await this.mediator.Send(new LoginCommand(request));

        return Ok(token);
    }
}
