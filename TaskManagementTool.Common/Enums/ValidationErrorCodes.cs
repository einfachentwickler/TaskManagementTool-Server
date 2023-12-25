namespace TaskManagementTool.Common.Enums;
public enum ValidationErrorCodes
{
    EmptyPassword = 0,
    WeakPassword,
    ConfirmPasswordDoesNotMatch,
    EmptyName,
    InvalidEmail,
    EmptyBody,
}