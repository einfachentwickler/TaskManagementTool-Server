using System.Collections.Generic;

namespace Application.Commands.Auth.ResetPassword.Models;

public record ResetPasswordResponse
{
    public required string Message { get; init; }
    public bool IsSuccess { get; init; }
    public required IEnumerable<string> Errors { get; init; }
}