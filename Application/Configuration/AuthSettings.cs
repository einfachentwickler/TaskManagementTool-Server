using System.ComponentModel.DataAnnotations;

namespace Application.Configuration;

public record AuthSettings
{
    [Required]
    public string Audience { get; init; }

    [Required]
    public string Issuer { get; init; }

    [Required]
    public string Key { get; init; }

    [Required]
    public int AccessTokenLifetimeMinutes { get; init; }
}