using Application.Configuration;
using Application.Dto.BuildJwtToken;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services.Jwt;

public class JwtSecurityTokenBuilder(IOptions<AuthSettings> config) : IJwtSecurityTokenBuilder
{
    public BuildJwtTokenResponse Build(string userId, string userRole, string userEmail)
    {
        var now = DateTime.UtcNow;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Email, userEmail),
            new(ClaimTypes.Role, userRole),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var keyBytes = Encoding.UTF8.GetBytes(config.Value.Key);

        var signingKey = new SymmetricSecurityKey(keyBytes);

        var expires = now.AddMinutes(config.Value.AccessTokenLifetimeMinutes);

        var token = new JwtSecurityToken(
            issuer: config.Value.Issuer,
            audience: config.Value.Audience,
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: new SigningCredentials(
                signingKey,
                SecurityAlgorithms.HmacSha256)
        );

        var JwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        return new BuildJwtTokenResponse
        {
            Token = JwtSecurityTokenHandler.WriteToken(token),
            Expires = expires
        };
    }
}