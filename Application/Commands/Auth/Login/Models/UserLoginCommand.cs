using MediatR;
using Newtonsoft.Json;

namespace Application.Commands.Auth.Login.Models;

public record UserLoginCommand : IRequest<UserLoginResponse>
{
    [JsonProperty("email")]
    public required string Email { get; init; }

    [JsonProperty("password")]
    public required string Password { get; init; }
}