using MediatR;
using Newtonsoft.Json;

namespace TaskManagementTool.BusinessLogic.Commands.Auth.Login.Models;

public class UserLoginRequest : IRequest<UserLoginResponse>
{
    [JsonProperty("email")]
    public string Email { get; init; }

    [JsonProperty("password")]
    public string Password { get; init; }
}