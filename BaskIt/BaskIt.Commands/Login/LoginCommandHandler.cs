using BaskIt.Domain.Entities;
using BaskIt.Services.Jwt;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BaskIt.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtService jwtService,
        ILogger<LoginCommandHandler> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var appUser = await _userManager.FindByEmailAsync(request.LoginRequest.Email);
        if (appUser == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(appUser, request.LoginRequest.Password, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            _logger.LogInformation("User {Email} logged in", request.LoginRequest.Email);
            var token = _jwtService.GenerateJwtToken(appUser.Email ?? string.Empty);
            return token;
        }

        throw new UnauthorizedAccessException("Invalid email or password");
    }
}
