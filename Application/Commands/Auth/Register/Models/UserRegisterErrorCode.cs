namespace Application.Commands.Auth.Register.Models;

public enum UserRegisterErrorCode
{
    InvalidEmail,
    InvalidPassword,
    WeakPassword,
    TextLengthExceeded,
    InvalidConfirmPassword,
    ConfirmPasswordDoesNotMatch,
    EmptyFirstName,
    EmptyLastName,
    UserAlreadyExists,
    InternalServerError
}