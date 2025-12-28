using Application.Queries.Admin.GetUsers.Models;

namespace Application.Queries.Admin.GetTodos.Models;

public record TodoDtoWithUser
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required string Content { get; init; }

    public required bool IsCompleted { get; init; }

    public required int Importance { get; init; }

    public required GetUserDto Creator { get; init; }
}