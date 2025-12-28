using MediatR;

namespace Application.Queries.Home.GetTodoById.Models;

public record GetTodoByIdQuery : IRequest<GetTodoByIdResponse>
{
    public required int TodoId { get; init; }
}