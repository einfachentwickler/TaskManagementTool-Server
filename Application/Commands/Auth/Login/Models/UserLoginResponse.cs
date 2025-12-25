using Newtonsoft.Json;
using System;

namespace Application.Commands.Auth.Login.Models;

public record UserLoginResponse
{
    [JsonProperty("token")]
    public required string Token { get; init; }

    [JsonProperty("expirationDate")]
    public DateTime ExpirationDate { get; init; }
}