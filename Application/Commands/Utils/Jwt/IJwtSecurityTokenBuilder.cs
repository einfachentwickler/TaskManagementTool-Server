using Application.Commands.Auth.Login.Models;
using Infrastructure.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Commands.Utils.Jwt;

public interface IJwtSecurityTokenBuilder
{
    (string, JwtSecurityToken) Build(UserEntity user, UserLoginCommand model);
}