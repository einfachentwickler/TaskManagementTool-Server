namespace Application.Commands.Auth.Login.Models;

public enum UserLoginErrorCode
{
    EmptyValue,
    InvalidEmail,
    LengthExceeded,
    UserNotFound,
    BlockedEmail,
    InvalidCredentials
}