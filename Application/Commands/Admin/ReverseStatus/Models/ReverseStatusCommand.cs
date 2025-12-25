using MediatR;

namespace Application.Commands.Admin.ReverseStatus.Models;

public record ReverseStatusCommand : IRequest<Unit>
{
    public required string UserId { get; init; }
}