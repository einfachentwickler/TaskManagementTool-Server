using MediatR;

namespace Application.Commands.Home.CreateTodo.Models;

public record CreateTodoCommand : IRequest<CreateTodoResponse>
{
    public required CreateTodoDto CreateTodoDto { get; init; }
}