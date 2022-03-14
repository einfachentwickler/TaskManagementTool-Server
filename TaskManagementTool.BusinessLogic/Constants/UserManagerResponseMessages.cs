namespace TaskManagementTool.BusinessLogic.Constants
{
    public static class UserManagerResponseMessages
    {
        public const string USER_CREATED = "User successfully created";

        public const string USER_DOES_NOT_EXIST = "There is no user with this email";

        public const string BLOCKED_EMAIL = "This email was blocked";

        public const string INVALID_CREDENTIALS = "Incorrect login or password";

        public const string CONFIRM_PASSWORD_DOES_NOT_MATCH_PASSWORD = "Confirm password doesn't match the password";

        public const string USER_WAS_NOT_CREATED = "User was not created";
    }
}
