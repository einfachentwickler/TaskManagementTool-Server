using Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Admin.DeleteUser.Models;
using TaskManagementTool.BusinessLogic.Commands.Wrappers;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.BusinessLogic.Commands.Admin.DeleteUser;

public class DeleteUserHandler(IUserManagerWrapper userManager, ITaskManagementToolDbContext dbContext) : IRequestHandler<DeleteUserRequest, Unit>
{
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IUserManagerWrapper _userManager = userManager;

    public async Task<Unit> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new TaskManagementToolException(ApiErrorCode.UserNotFound, $"User with email {request.Email} was not found");

        await _dbContext.Todos.Where(todo => todo.Creator.Email == request.Email).ExecuteDeleteAsync(cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var identityResult = await _userManager.DeleteAsync(user);
        if (!identityResult.Succeeded)
        {
            string errors = string.Join("\n", identityResult.Errors);

            throw new TaskManagementToolException($"Update failed: {errors}");
        }

        return new Unit();
    }
}