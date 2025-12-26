using Application.Commands.Admin.ReverseStatus.Models;
using Application.Services.IdentityUserManagement;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Admin.ReverseStatus;

public class ReverseStatusHandler(IIdentityUserManagerWrapper userManager)
        : IRequestHandler<ReverseStatusCommand, Unit>
{
    private readonly IIdentityUserManagerWrapper _userManager = userManager;

    public async Task<Unit> Handle(
        ReverseStatusCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
        {
            throw new CustomException<ReverseStatusErrorCode>(
                ReverseStatusErrorCode.UserIdIsNullOrEmpty,
                ReverseStatusErrorMessages.UserIdIsNullOrEmpty);
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            throw new CustomException<ReverseStatusErrorCode>(
                ReverseStatusErrorCode.UserNotFound,
                ReverseStatusErrorMessages.UserNotFound);
        }

        user.IsBlocked = !user.IsBlocked;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            // TODO: log result.Errors
            throw new CustomException<ReverseStatusErrorCode>(
                ReverseStatusErrorCode.InternalServerError,
                ReverseStatusErrorMessages.InternalServerError);
        }

        return Unit.Value;
    }
}