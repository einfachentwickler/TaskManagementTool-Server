using System;

namespace Application.Commands.Auth.Login.Models;

public record UserLoginResponse
{
    public required string AccessToken { get; init; }

    public DateTime Expires { get; init; }

    public required string RefreshToken { get; init; }
}