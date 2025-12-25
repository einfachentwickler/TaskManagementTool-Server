namespace Application.Commands.Admin.DeleteUser.Models;

public enum DeleteUserErrorCode
{
    InvalidEmail,
    UserNotFound,
    InternalServerError
}