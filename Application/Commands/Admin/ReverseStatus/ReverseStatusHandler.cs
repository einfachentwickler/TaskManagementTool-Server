using Application.Commands.Admin.ReverseStatus.Models;
using Application.Services.IdentityUserManagement;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Admin.ReverseStatus;

public class ReverseStatusHandler(IIdentityUserManagerWrapper userManager, ILogger<ReverseStatusHandler> logger)
        : IRequestHandler<ReverseStatusCommand, Unit>
{
    private readonly IIdentityUserManagerWrapper _userManager = userManager;
    private readonly ILogger<ReverseStatusHandler> _logger = logger;

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
            _logger.LogWarning("User status revert failed, errors: {0}", JsonConvert.SerializeObject(result));

            throw new CustomException<ReverseStatusErrorCode>(
                ReverseStatusErrorCode.InternalServerError,
                ReverseStatusErrorMessages.InternalServerError);
        }

        return Unit.Value;
    }
}