using BaskIt.Domain.Entities;
using BaskIt.Services.Jwt;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BaskIt.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(UserManager<ApplicationUser> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            UserName = request.RegisterRequest.Email,
            Email = request.RegisterRequest.Email,
            FirstName = request.RegisterRequest.FirstName ?? string.Empty,
            LastName = request.RegisterRequest.LastName ?? string.Empty
        };

        var result = await _userManager.CreateAsync(user, request.RegisterRequest.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"User registration failed: {errors}");
        }

        var token = _jwtService.GenerateJwtToken(user.Email);

        return token;
    }
}
