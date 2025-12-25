namespace Infrastructure.ConfigurationModels;

public record DefaultAdminCredentials
{
    public required string Email { get; init; }

    public required string Password { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public int Age { get; init; }

    public required string Role { get; init; }
}