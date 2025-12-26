namespace Application.Configuration;

public record TokenValidationOptions
{
    public bool ValidateIssuer { get; init; }
    public bool ValidateAudience { get; init; }
    public bool RequireExpirationTime { get; init; }
    public bool ValidateIssuerSigningKey { get; init; }
}