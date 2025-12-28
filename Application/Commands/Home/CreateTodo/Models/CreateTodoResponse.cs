namespace Application.Commands.Home.CreateTodo.Models;

public record CreateTodoResponse
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required string Content { get; init; }

    public required bool IsCompleted { get; init; }

    public required int Importance { get; init; }
}