using Application.Commands.Admin.DeleteTodoByAdmin.Models;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Admin.DeleteTodoByAdmin;

public class DeleteTodoByAdminHandler(ITaskManagementToolDbContext dbContext) : IRequestHandler<DeleteTodoByAdminRequest, Unit>
{
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;

    public async Task<Unit> Handle(DeleteTodoByAdminRequest request, CancellationToken cancellationToken)
    {
        await _dbContext.Todos.Where(entity => entity.Id == request.TodoId).ExecuteDeleteAsync(cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Unit();
    }
}