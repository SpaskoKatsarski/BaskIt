using BaskIt.API.Data;
using BaskIt.API.Models;
using BaskIt.Services.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BaskIt.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly ILogger<AuthController> logger;
    private readonly IJwtService jwtService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AuthController> logger,
        IJwtService authService)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.logger = logger;
        this.jwtService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName ?? string.Empty,
            LastName = request.LastName ?? string.Empty
        };

        var result = await this.userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            this.logger.LogInformation("User {Email} created successfully", request.Email);

            return Ok(new
            {
                message = "Registration successful",
                userId = user.Id,
                email = user.Email
            });
        }

        return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var appUser = await this.userManager.FindByEmailAsync(user.Email);
        if (appUser == null)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        var result = await this.signInManager.CheckPasswordSignInAsync(appUser, user.Password, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            this.logger.LogInformation("User {Email} logged in", user.Email);
            var token = this.jwtService.GenerateJwtToken(appUser.Email ?? string.Empty);
            return Ok(token);
        }

        return Unauthorized(new { message = "Invalid email or password" });
    }
}
