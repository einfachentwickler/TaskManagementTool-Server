using MediatR;
using Newtonsoft.Json;

namespace TaskManagementTool.BusinessLogic.Commands.Auth.Register.Models;

public class UserRegisterRequest : IRequest<UserRegisterResponse>
{
    [JsonProperty("email")]
    public string Email { get; init; }

    [JsonProperty("password")]
    public string Password { get; init; }

    [JsonProperty("confirmPassword")]
    public string ConfirmPassword { get; init; }

    [JsonProperty("firstName")]
    public string FirstName { get; init; }

    [JsonProperty("lastName")]
    public string LastName { get; init; }

    [JsonProperty("age")]
    public int Age { get; init; }
}
