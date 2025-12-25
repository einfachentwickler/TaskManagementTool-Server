namespace Application.Commands.Auth.ResetPassword.Models;

public enum ResetPasswordErrorCode
{
    InvalidCurrentPassword,
    InvalidNewPassword,
    InvalidConfirmNewPassword,
    InvalidEmail,
    UserNotFound
}