namespace Application.Queries.Admin.GetUsers.Models;

public record GetUserDto
{
    public required string Id { get; init; }

    public required string Email { get; init; }

    public required string Username { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required int Age { get; init; }

    public required bool IsBlocked { get; init; }

    public required string Role { get; init; }
}