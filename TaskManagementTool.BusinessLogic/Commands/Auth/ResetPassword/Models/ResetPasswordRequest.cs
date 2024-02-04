using MediatR;

namespace TaskManagementTool.BusinessLogic.Commands.Auth.ResetPassword.Models;

public class ResetPasswordRequest : IRequest<ResetPasswordResponse>
{
    public string Email { get; init; }
    public string CurrentPassword { get; init; }
    public string NewPassword { get; init; }
    public string ConfirmNewPassword { get; init; }
}