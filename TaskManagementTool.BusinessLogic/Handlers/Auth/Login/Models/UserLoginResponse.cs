using System;
using System.Collections.Generic;

namespace TaskManagementTool.BusinessLogic.Handlers.Auth.Login.Models;

public class UserLoginResponse
{
    public bool IsSuccess { get; init; }

    public string Token { get; init; }

    public string Message { get; init; } = string.Empty;

    public DateTime ExpirationDate { get; init; }

    public IEnumerable<string> Errors { get; set; } = new List<string>();
}