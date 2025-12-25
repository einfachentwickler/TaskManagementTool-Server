using MediatR;

namespace Application.Commands.Admin.DeleteUser.Models;

public class DeleteUserRequest : IRequest<Unit>
{
    public string Email { get; set; }
}