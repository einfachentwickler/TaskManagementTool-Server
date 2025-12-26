using Application.Dto.BuildJwtToken;

namespace Application.Services.Jwt;

public interface IJwtSecurityTokenBuilder
{
    BuildJwtTokenResponse Build(string userId, string userRole, string userEmail);
}