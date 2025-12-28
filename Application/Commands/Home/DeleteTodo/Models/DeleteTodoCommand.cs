using MediatR;

namespace Application.Commands.Home.DeleteTodo.Models;

public record DeleteTodoCommand : IRequest<Unit>
{
    public required int TodoId { get; init; }
}