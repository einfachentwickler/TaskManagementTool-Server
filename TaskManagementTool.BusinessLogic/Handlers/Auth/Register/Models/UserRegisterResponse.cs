using System.Collections.Generic;

namespace TaskManagementTool.BusinessLogic.Handlers.Auth.Register.Models;

public class UserRegisterResponse
{
    public bool IsSuccess { get; init; }

    public string Message { get; init; }

    public IEnumerable<string> Errors { get; set; } = new List<string>();
}