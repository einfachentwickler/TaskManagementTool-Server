namespace Shared.Configuration;

public record ConcurrencyRateLimiterOptions
{
    public int PermitLimit { get; init; }
    public int QueueLimit { get; init; }
}