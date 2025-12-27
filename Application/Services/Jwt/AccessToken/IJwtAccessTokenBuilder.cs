using Application.Dto.BuildJwtToken;

namespace Application.Services.Jwt.AccessToken;

public interface IJwtAccessTokenBuilder
{
    BuildJwtTokenResponse Build(string userId, string userRole, string userEmail);
}