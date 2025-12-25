using Infrastructure.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using TaskManagementTool.BusinessLogic.Commands.Auth.Login.Models;

namespace TaskManagementTool.BusinessLogic.Commands.Utils.Jwt;

public interface IJwtSecurityTokenBuilder
{
    (string, JwtSecurityToken) Build(UserEntity user, UserLoginRequest model);
}