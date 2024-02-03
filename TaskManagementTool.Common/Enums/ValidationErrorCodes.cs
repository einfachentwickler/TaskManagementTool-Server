namespace TaskManagementTool.Common.Enums;

public enum ValidationErrorCodes
{
    EmptyPassword = 0,
    WeakPassword,
    ConfirmPasswordDoesNotMatch,
    InvalidEmail,
    EmptyValue,
    TextLengthExceeded
}