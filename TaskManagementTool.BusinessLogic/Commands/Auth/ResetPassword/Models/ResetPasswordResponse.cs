using System.Collections.Generic;

namespace TaskManagementTool.BusinessLogic.Commands.Auth.ResetPassword.Models;

public class ResetPasswordResponse
{
    public string Message { get; set; } = "";
    public bool IsSuccess { get; set; }
    public IEnumerable<string> Errors { get; set; } = new List<string>();
}