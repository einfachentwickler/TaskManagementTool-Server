using Application.Commands.Admin.ReverseStatus.Models;
using Application.Commands.Wrappers;
using Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.Common.Exceptions;

namespace Application.Commands.Admin.ReverseStatus;

public class ReverseStatusHandler(IUserManagerWrapper userManager) : IRequestHandler<ReverseStatusCommand, Unit>
{
    public async Task<Unit> Handle(ReverseStatusCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
        {
            throw new CustomException<ReverseStatusErrorCode>(ReverseStatusErrorCode.UserIdIsNullOrEmpty, ReverseStatusErrorMessages.UserIdIsNullOrEmpty);
        }

        var user = await userManager.Users.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            throw new CustomException<ReverseStatusErrorCode>(ReverseStatusErrorCode.UserNotFound, ReverseStatusErrorMessages.UserNotFound);
        }

        user.IsBlocked = !user.IsBlocked;

        UserEntity entityToUpdate = await CopyAsync(user);

        IdentityResult identityResult = await userManager.UpdateAsync(entityToUpdate);

        if (!identityResult.Succeeded)
        {
            throw new CustomException<ReverseStatusErrorCode>(ReverseStatusErrorCode.InternalServerError, ReverseStatusErrorMessages.InternalServerError);
            //todo log errors
        }

        return new Unit();
    }

    private async Task<UserEntity> CopyAsync(UserEntity user)
    {
        //todo resolve this mess
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