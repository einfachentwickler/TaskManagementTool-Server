using MediatR;

namespace Application.Commands.Home.UpdateTodo.Models;

public record UpdateTodoCommand : IRequest<UpdateTodoResponse>
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Content { get; init; }
    public bool IsCompleted { get; init; }
    public int Importance { get; init; }
}