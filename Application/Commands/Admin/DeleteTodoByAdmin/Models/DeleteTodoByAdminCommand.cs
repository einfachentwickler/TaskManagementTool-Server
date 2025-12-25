using MediatR;

namespace Application.Commands.Admin.DeleteTodoByAdmin.Models;

public record DeleteTodoByAdminCommand : IRequest<Unit>
{
    public int TodoId { get; init; }
}