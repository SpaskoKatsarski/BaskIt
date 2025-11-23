using BaskIt.Domain.Entities;
using BaskIt.Services.Jwt;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BaskIt.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(UserManager<ApplicationUser> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (!string.Equals(request.RegisterRequest.Password, request.RegisterRequest.ConfirmPassword))
        {
            throw new ArgumentException("Passwords do not match.");
        }

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
    }
}
