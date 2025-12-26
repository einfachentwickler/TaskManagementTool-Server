using System;

namespace Application.Dto.BuildJwtToken;

public record BuildJwtTokenResponse
{
    public required string Token { get; init; }
    public DateTime Expires { get; init; }
}