namespace Application.Commands.Auth.Login.Models;

public static class UserLoginErrorMessages
{
    public const string EmptyValue = "The field cannot be empty.";
    public const string InvalidEmail = "The email format is invalid.";
    public const string LengthExceeded = "The length of the input exceeds the allowed limit.";
    public const string PasswordRequired = "The field cannot be empty.";
    public const string UserNotFound = "No user found with the provided email.";
    public const string BlockedEmail = "The email is blocked.";
    public const string InvalidCredentials = "The provided credentials are invalid.";
}