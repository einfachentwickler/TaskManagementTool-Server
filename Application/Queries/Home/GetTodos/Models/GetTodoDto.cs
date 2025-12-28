namespace Application.Queries.Home.GetTodos.Models;

public class GetTodoDto
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required string Content { get; init; }

    public required bool IsCompleted { get; init; }

    public required int Importance { get; init; }
}