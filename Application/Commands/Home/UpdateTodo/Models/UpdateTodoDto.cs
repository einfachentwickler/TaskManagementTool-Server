namespace Application.Commands.Home.UpdateTodo.Models;

public record UpdateTodoDto
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Content { get; init; }
    public bool IsCompleted { get; init; }
    public int Importance { get; init; }
}