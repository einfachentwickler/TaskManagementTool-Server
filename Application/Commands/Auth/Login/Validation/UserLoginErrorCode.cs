namespace Application.Commands.Auth.Login.Validation;

public enum UserLoginErrorCode
{
    EmptyValue,
    InvalidEmail,
    LengthExceeded,
    UserNotFound,
    BlockedEmail,
    InvalidCredentials
}