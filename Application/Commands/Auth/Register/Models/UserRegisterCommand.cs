using MediatR;

namespace Application.Commands.Auth.Register.Models;

public record UserRegisterCommand : IRequest<Unit>
{
    public required string Email { get; init; }

    public required string Password { get; init; }

    public required string ConfirmPassword { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required int Age { get; init; }
}