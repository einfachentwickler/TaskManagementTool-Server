using MediatR;

namespace Application.Commands.Home.DeleteTodo.Models;

public record DeleteTodoCommand : IRequest<Unit>
{
    public int TodoId { get; init; }
}