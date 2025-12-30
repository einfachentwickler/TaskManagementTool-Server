using System;

namespace Application.Dto.BuildJwtToken;

public record BuildJwtTokenResponse
{
    public required string Token { get; init; }
    public required DateTime Expires { get; init; }
}