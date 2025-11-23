using BaskIt.Shared.DTOs.Auth;
using MediatR;

namespace BaskIt.Commands.Register;

public record RegisterCommand(RegisterRequest RegisterRequest) : IRequest;
