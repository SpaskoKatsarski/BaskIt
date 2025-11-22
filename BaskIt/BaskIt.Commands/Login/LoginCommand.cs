using BaskIt.Shared.DTOs.Auth;
using MediatR;

namespace BaskIt.Commands.Login;

public record LoginCommand(LoginRequest LoginRequest) : IRequest<string>;
