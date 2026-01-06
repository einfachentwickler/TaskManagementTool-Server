using Application.Dto.BuildJwtToken;
using Application.Services.Abstractions.DateTimeGeneration;
using Application.Services.Abstractions.GuidGeneration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services.Jwt.AccessToken;

public class JwtAccessTokenBuilder(
    IOptions<AuthOptions> config,
    IDateTimeProvider dateTimeProvider,
    IGuidProvider guidProvider) : IJwtAccessTokenBuilder
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IGuidProvider _guidProvider = guidProvider;

    public BuildJwtTokenResponse Build(string userId, string userRole, string userEmail)
    {
        var utcNow = _dateTimeProvider.UtcNow;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Email, userEmail),
            new(ClaimTypes.Role, userRole),
            new(JwtRegisteredClaimNames.Jti, _guidProvider.NewGuid.ToString()),
            new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(utcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(utcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var keyBytes = Encoding.UTF8.GetBytes(config.Value.Key);

        var signingKey = new SymmetricSecurityKey(keyBytes);

        var expires = utcNow.AddMinutes(config.Value.AccessTokenLifetimeMinutes);

        var token = new JwtSecurityToken(
            issuer: config.Value.Issuer,
            audience: config.Value.Audience,
            claims: claims,
            notBefore: utcNow,
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