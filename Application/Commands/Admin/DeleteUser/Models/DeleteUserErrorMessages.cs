namespace Application.Commands.Admin.DeleteUser.Models;

public static class DeleteUserErrorMessages
{
    public const string UserNotFound = "User with the specified email does not exist.";
    public const string InvalidEmail = "The provided email address is not valid.";
    public const string InternalServerError = "An unexpected error occurred while attempting to delete the user.";
}