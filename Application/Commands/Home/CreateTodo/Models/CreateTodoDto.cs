namespace Application.Commands.Home.CreateTodo.Models;

public record CreateTodoDto
{
    public required string Name { get; init; }

    public required string Content { get; init; }

    public int Importance { get; init; }
}