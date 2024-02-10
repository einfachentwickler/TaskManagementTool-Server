namespace IntegrationTests.Constants;
public static class UriConstants
{
    public const string AUTH_REGISTER_URI = "https://localhost:44311/api/auth/register";

    public const string AUTH_LOGIN_URI = "https://localhost:44311/api/auth/login";

    public const string AUTH_RESET_PASSWORD_URI = "https://localhost:44311/api/auth/reset-password/";

    public const string HOME_GET_USER_TODOS = "https://localhost:44311/api/home?pageSize={0}&pageNumber={1}";

    public const string HOME_CREATE_TODO_URI = "https://localhost:44311/api/home";

    public const string HOME_UPDATE_TODO_URI = "https://localhost:44311/api/home";

    public const string HOME_GET_TODO_URI = "https://localhost:44311/api/home/";

    public const string HOME_DELETE_TODO_URI = "https://localhost:44311/api/home/";

    public const string ADMIN_REVERSE_STATUS_URI = "https://localhost:44311/api/admin/reverse-status/";

    public const string ADMIN_GET_USERS_URI = "https://localhost:44311/api/admin/users";

    public const string ADMIN_GET_TODOS_URI = "https://localhost:44311/api/admin/todos";

    public const string ADMIN_DELETE_USER_URI = "https://localhost:44311/api/admin/users/";
}