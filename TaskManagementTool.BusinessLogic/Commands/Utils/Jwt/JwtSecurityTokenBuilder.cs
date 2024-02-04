using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementTool.BusinessLogic.Commands.Auth.Login.Models;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Commands.Utils.Jwt;

public class JwtSecurityTokenBuilder(IConfiguration configuration) : IJwtSecurityTokenBuilder
{
    public (string, JwtSecurityToken) Build(User user, UserLoginRequest model)
    {
        Claim[] claims = [
            new("email", model.Email),
            new(ClaimTypes.NameIdentifier, user.Id),
            new("role", user.Role)
        ];

        byte[] authKey = Encoding.UTF8.GetBytes(configuration["AuthSettings:Key"]);

        SymmetricSecurityKey key = new(authKey);

        JwtSecurityToken token = new(
            configuration["AuthSettings:Issuer"],
            configuration["AuthSettings:Audience"],
            claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

        return (tokenAsString, token);
    }
}