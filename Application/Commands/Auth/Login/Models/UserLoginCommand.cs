using MediatR;
using Newtonsoft.Json;

namespace Application.Commands.Auth.Login.Models;

public record UserLoginCommand : IRequest<UserLoginResponse>
{
    public required string Email { get; init; }

    public required string Password { get; init; }
}