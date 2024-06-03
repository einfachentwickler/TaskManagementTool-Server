using Newtonsoft.Json;
using System.Collections.Generic;

namespace TaskManagementTool.BusinessLogic.Commands.Auth.Register.Models;

public class UserRegisterResponse
{
    [JsonProperty("isSuccess")]
    public bool IsSuccess { get; init; }

    [JsonProperty("message")]
    public string Message { get; init; }

    [JsonProperty("errors")]
    public IEnumerable<string> Errors { get; set; } = [];
}