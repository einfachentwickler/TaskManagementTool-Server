using Newtonsoft.Json;
using System.Collections.Generic;

namespace Application.Commands.Auth.Register.Models;

public record UserRegisterResponse
{
    [JsonProperty("isSuccess")]
    public bool IsSuccess { get; init; }

    [JsonProperty("message")]
    public required string Message { get; init; }

    [JsonProperty("errors")]
    public required IEnumerable<string> Errors { get; init; }
}