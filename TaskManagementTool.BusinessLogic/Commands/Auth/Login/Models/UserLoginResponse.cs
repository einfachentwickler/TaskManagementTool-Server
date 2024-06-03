using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TaskManagementTool.BusinessLogic.Commands.Auth.Login.Models;

public class UserLoginResponse
{
    [JsonProperty("isSuccess")]
    public bool IsSuccess { get; init; }

    [JsonProperty("token")]
    public string Token { get; init; }

    [JsonProperty("message")]
    public string Message { get; init; } = string.Empty;

    [JsonProperty("expirationDate")]
    public DateTime ExpirationDate { get; init; }

    [JsonProperty("errors")]
    public IEnumerable<string> Errors { get; set; } = new List<string>();
}