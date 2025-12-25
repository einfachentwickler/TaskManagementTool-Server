using Infrastructure.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Admin.DeleteUser.Models;
using TaskManagementTool.BusinessLogic.Commands.Wrappers;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.BusinessLogic.Commands.Admin.DeleteUser;

public class DeleteUserHandler(IUserManagerWrapper userManager, ITodoRepository todoRepository) : IRequestHandler<DeleteUserRequest, Unit>
{
    public async Task<Unit> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email)
            ?? throw new TaskManagementToolException(ApiErrorCode.UserNotFound, $"User with email {request.Email} was not found");

        await todoRepository.DeleteAsync(todo => todo.Creator.Email == request.Email);

        var identityResult = await userManager.DeleteAsync(user);
        if (!identityResult.Succeeded)
        {
            string errors = string.Join("\n", identityResult.Errors);

            throw new TaskManagementToolException($"Update failed: {errors}");
        }

        return new Unit();
    }
}