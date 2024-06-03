using System.IdentityModel.Tokens.Jwt;
using TaskManagementTool.BusinessLogic.Commands.Auth.Login.Models;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Commands.Utils.Jwt;

public interface IJwtSecurityTokenBuilder
{
    (string, JwtSecurityToken) Build(UserEntry user, UserLoginRequest model);
}