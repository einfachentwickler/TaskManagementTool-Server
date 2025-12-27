using System.ComponentModel.DataAnnotations;

namespace Shared.Configuration;

public record AuthOptions
{
    [Required]
    public string Audience { get; init; }

    [Required]
    public string Issuer { get; init; }

    [Required]
    public string Key { get; init; }

    [Required]
    public int AccessTokenLifetimeMinutes { get; init; }

    [Required]
    public int RefreshTokenLifetimeDays { get; init; }
}