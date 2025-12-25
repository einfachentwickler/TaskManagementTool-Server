namespace Application.Commands.Auth.Register.Models;

public class UserRegisterErrorMessages
{
    public const string InvalidEmail = "The email address is not valid.";
    public const string InvalidPassword = "The password does not meet the required criteria.";
    public const string WeakPassword = "The password is too weak.";
    public const string TextLengthExceeded = "The text length exceeds the allowed limit.";
    public const string InvalidConfirmPassword = "The confirm password does not meet the required criteria.";
    public const string ConfirmPasswordDoesNotMatch = "The confirm password does not match the password.";
    public const string EmptyFirstName = "The first name cannot be empty.";
    public const string EmptyLastName = "The last name cannot be empty.";
    public const string InternalServerError = "An internal server error occurred while creating the user.";
}