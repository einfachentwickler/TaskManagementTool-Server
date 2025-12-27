using System;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services.Jwt.RefreshToken;

public class JwtRefreshTokenGenerator : IJwtRefreshTokenGenerator
{
    public string Generate()
    {
        var bytes = new byte[64];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        return Convert.ToBase64String(bytes);
    }

    public string Hash(string token)
    {
        return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
    }
}