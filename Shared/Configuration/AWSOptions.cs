namespace Shared.Configuration;

public record AWSOptions
{
    public required string AccessKey { get; init; }

    public required string SecretKey { get; init; }

    public required string LogGroup { get; init; }

    public required string LogStreamPrefix { get; init; }

    public required string MinLogLevel { get; init; }

    public required string LogsRetentionPolicy { get; init; }
}