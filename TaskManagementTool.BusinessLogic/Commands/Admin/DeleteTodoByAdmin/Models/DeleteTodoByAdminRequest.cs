using MediatR;

namespace TaskManagementTool.BusinessLogic.Commands.Admin.DeleteTodoByAdmin.Models;

public class DeleteTodoByAdminRequest : IRequest<Unit>
{
    public int TodoId { get; init; }
}