using Application.Commands.Auth.Login.Models;
using MediatR;

namespace Application.Commands.Auth.RefreshToken.Models;

public record RefreshTokenCommand : IRequest<UserLoginResponse>
{
    public required string RefreshToken { get; init; }
}