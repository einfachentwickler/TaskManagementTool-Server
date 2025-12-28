using MediatR;

namespace Application.Commands.Home.CreateTodo.Models;

public record CreateTodoCommand : IRequest<CreateTodoResponse>
{
    public required string Name { get; init; }

    public required string Content { get; init; }

    public required int Importance { get; init; }
}