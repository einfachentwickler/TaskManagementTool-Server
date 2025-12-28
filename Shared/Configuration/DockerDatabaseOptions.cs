namespace Shared.Configuration;

public record DockerDatabaseOptions
{
    public required string DBServer { get; init; }

    public required string DBPort { get; init; }

    public required string DBName { get; init; }

    public required string DbNameLogs { get; init; }

    public required string DBUser { get; init; }

    public required string DBPassword { get; init; }
}