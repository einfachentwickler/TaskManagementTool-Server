﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementTool.BusinessLogic.Commands.Auth.Login.Models;
using TaskManagementTool.Common.Configuration;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Commands.Utils.Jwt;

public class JwtSecurityTokenBuilder(IOptions<AuthSettings> config) : IJwtSecurityTokenBuilder
{
    public (string, JwtSecurityToken) Build(UserEntry user, UserLoginRequest model)
    {
        Claim[] claims = [
            new("email", model.Email),
            new(ClaimTypes.NameIdentifier, user.Id),
            new("role", user.Role)
        ];

        byte[] authKey = Encoding.UTF8.GetBytes(config.Value.Key);

        SymmetricSecurityKey key = new(authKey);

        JwtSecurityToken token = new(
            config.Value.Issuer,
            config.Value.Audience,
            claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

        return (tokenAsString, token);
    }
}