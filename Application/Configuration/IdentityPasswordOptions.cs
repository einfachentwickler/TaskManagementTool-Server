namespace Application.Configuration;

public record IdentityPasswordOptions
{
    public int RequiredLength { get; init; }
    public bool RequireDigit { get; init; }
    public bool RequireLowercase { get; init; }
    public bool RequireNonAlphanumeric { get; init; }
    public int RequiredUniqueChars { get; init; }
    public bool RequireUppercase { get; init; }
}