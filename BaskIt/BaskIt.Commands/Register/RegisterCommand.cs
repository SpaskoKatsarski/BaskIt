using MediatR;

namespace BaskIt.Commands.Register;

public record RegisterCommand(string Email, string Password) : IRequest<string>;
