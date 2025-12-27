namespace Application.Services.Jwt.RefreshToken;

public interface IJwtRefreshTokenGenerator
{
    string Generate();
    string Hash(string token);
}