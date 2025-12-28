using MediatR;

namespace Application.Commands.Home.UpdateTodo.Models;

public record UpdateTodoCommand : IRequest<UpdateTodoResponse>
{
    public required UpdateTodoDto UpdateTodoDto { get; init; }
}