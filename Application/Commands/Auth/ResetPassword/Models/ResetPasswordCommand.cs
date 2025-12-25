using MediatR;

namespace Application.Commands.Auth.ResetPassword.Models;

public record ResetPasswordCommand : IRequest<ResetPasswordResponse>
{
    public required string Email { get; init; }

    public required string CurrentPassword { get; init; }

    public required string NewPassword { get; init; }

    public required string ConfirmNewPassword { get; init; }
}