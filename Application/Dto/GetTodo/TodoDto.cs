using Application.Queries.Admin.GetUsers.Models;

namespace Application.Dto.GetTodo;

public record TodoDto
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public required string Content { get; init; }

    public bool IsCompleted { get; init; }

    public int Importance { get; init; }

    public required GetUserDto Creator { get; init; }
}