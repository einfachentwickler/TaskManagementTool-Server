using MediatR;

namespace Application.Commands.Admin.DeleteTodoByAdmin.Models;

public class DeleteTodoByAdminRequest : IRequest<Unit>
{
    public int TodoId { get; init; }
}