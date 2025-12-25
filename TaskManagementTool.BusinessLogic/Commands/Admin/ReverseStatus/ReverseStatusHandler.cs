using Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Admin.ReverseStatus.Models;
using TaskManagementTool.BusinessLogic.Commands.Wrappers;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.BusinessLogic.Commands.Admin.ReverseStatus;

public class ReverseStatusHandler(IUserManagerWrapper userManager) : IRequestHandler<ReverseStatusRequest, Unit>
{
    public async Task<Unit> Handle(ReverseStatusRequest request, CancellationToken cancellationToken)
    {
        UserEntity user = await userManager.Users.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

        user.IsBlocked = !user.IsBlocked;

        UserEntity entityToUpdate = await CopyAsync(user);

        IdentityResult identityResult = await userManager.UpdateAsync(entityToUpdate);

        if (!identityResult.Succeeded)
        {
            string errors = string.Join("\n", identityResult.Errors);

            throw new TaskManagementToolException($"Update failed: {errors}");
        }

        return new Unit();
    }

    private async Task<UserEntity> CopyAsync(UserEntity user)
    {
        UserEntity userTemp = await userManager.FindByEmailAsync(user.Email);

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