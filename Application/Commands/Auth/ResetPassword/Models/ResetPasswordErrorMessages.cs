namespace Application.Commands.Auth.ResetPassword.Models;

public static class ResetPasswordErrorMessages
{
    public const string InvalidCurrentPassword = "The provided current password is invalid.";
    public const string InvalidNewPassword = "The provided new password does not meet the required criteria.";
    public const string InvalidConfirmNewPassword = "The confirmation of the new password does not match.";
    public const string InvalidEmail = "The provided email address is not valid.";
    public const string UserNotFound = "No user found with the provided email address.";
    public const string PasswordsDoNotMatch = "The new password and confirmation password do not match.";
}