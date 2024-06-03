using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Admin.DeleteTodoByAdmin.Models;
using TaskManagementTool.DataAccess.Contracts;

namespace TaskManagementTool.BusinessLogic.Commands.Admin.DeleteTodoByAdmin;

public class DeleteTodoByAdminHandler(ITodoRepository todoRepository) : IRequestHandler<DeleteTodoByAdminRequest, Unit>
{
    public async Task<Unit> Handle(DeleteTodoByAdminRequest request, CancellationToken cancellationToken)
    {
        await todoRepository.DeleteAsync(entry => entry.Id == request.TodoId);

        return new Unit();
    }
}