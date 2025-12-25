using MediatR;
using Newtonsoft.Json;

namespace Application.Commands.Auth.Login.Models;

public class UserLoginRequest : IRequest<UserLoginResponse>
{
    [JsonProperty("email")]
    public string Email { get; init; }

    [JsonProperty("password")]
    public string Password { get; init; }
}