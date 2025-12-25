using MediatR;

namespace Application.Commands.Admin.DeleteUser.Models;

public record DeleteUserCommand : IRequest<Unit>
{
    public required string Email { get; init; }
}