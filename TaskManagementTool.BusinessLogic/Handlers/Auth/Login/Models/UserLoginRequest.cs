using MediatR;

namespace TaskManagementTool.BusinessLogic.Handlers.Auth.Login.Models;

public class UserLoginRequest : IRequest<UserLoginResponse>
{
    public string Email { get; init; }

    public string Password { get; init; }
}