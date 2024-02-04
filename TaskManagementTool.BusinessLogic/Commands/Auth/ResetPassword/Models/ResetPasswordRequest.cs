using MediatR;
using Newtonsoft.Json;

namespace TaskManagementTool.BusinessLogic.Commands.Auth.ResetPassword.Models;

public class ResetPasswordRequest : IRequest<ResetPasswordResponse>
{
    [JsonProperty("email")]
    public string Email { get; init; }

    [JsonProperty("currentPassword")]
    public string CurrentPassword { get; init; }

    [JsonProperty("newPassword")]
    public string NewPassword { get; init; }

    [JsonProperty("confirmNewPassword")]
    public string ConfirmNewPassword { get; init; }
}