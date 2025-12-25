using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Application.Commands.Auth.Login.Models;

public record UserLoginResponse
{
    [JsonProperty("isSuccess")]
    public bool IsSuccess { get; init; }

    [JsonProperty("token")]
    public required string Token { get; init; }

    [JsonProperty("message")]
    public required string Message { get; init; }

    [JsonProperty("expirationDate")]
    public DateTime ExpirationDate { get; init; }

    [JsonProperty("errors")]
    public required IEnumerable<string> Errors { get; init; }
}