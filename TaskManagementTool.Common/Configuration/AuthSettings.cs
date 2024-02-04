using System.ComponentModel.DataAnnotations;

namespace TaskManagementTool.Common.Configuration;

public class AuthSettings()
{
    [Required]
    public string Audience { get; init; }

    [Required]
    public string Issuer { get; init; }

    [Required]
    public string Key { get; init; }
}