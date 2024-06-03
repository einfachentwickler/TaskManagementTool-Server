using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Admin.ReverseStatus.Models;
using TaskManagementTool.BusinessLogic.Commands.Wrappers;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Commands.Admin.ReverseStatus;

public class ReverseStatusHandler(IUserManagerWrapper userManager) : IRequestHandler<ReverseStatusRequest, Unit>
{
    public async Task<Unit> Handle(ReverseStatusRequest request, CancellationToken cancellationToken)
    {
        UserEntry user = await userManager.Users.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

        user.IsBlocked = !user.IsBlocked;

        UserEntry entityToUpdate = await CopyAsync(user);

        IdentityResult identityResult = await userManager.UpdateAsync(entityToUpdate);

        if (!identityResult.Succeeded)
        {
            string errors = string.Join("\n", identityResult.Errors);

            throw new TaskManagementToolException($"Update failed: {errors}");
        }

        return new Unit();
    }

    private async Task<UserEntry> CopyAsync(UserEntry user)
    {
        UserEntry userTemp = await userManager.FindByEmailAsync(user.Email);

        userTemp.Id = user.Id;
        userTemp.Age = user.Age;
        userTemp.FirstName = user.FirstName;
        userTemp.LastName = user.LastName;
        userTemp.IsBlocked = user.IsBlocked;
        userTemp.Role = user.Role;
        userTemp.Email = user.Email;
        userTemp.UserName = user.UserName;

        return userTemp;
    }
}