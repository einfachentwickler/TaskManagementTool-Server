using System.ComponentModel.DataAnnotations;

namespace TaskManagementTool.Common.Configuration;

public record AuthSettings()
{
    //todo check options pattern. Attributes vs required
    [Required]
    public required string Audience { get; init; }

    [Required]
    public required string Issuer { get; init; }

    [Required]
    public required string Key { get; init; }
}