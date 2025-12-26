using Application.Commands.Auth.ResetPassword.Models;
using FluentValidation;
using Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Auth.ResetPassword;

public class ResetPasswordHandler(
    IValidator<ResetPasswordCommand> validator,
    UserManager<UserEntity> userManager
    ) : IRequestHandler<ResetPasswordCommand, Unit>
{
    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var firstError = validationResult.Errors[0];
            throw new CustomException<ResetPasswordErrorCode>(Enum.Parse<ResetPasswordErrorCode>(firstError.ErrorCode), firstError.ErrorMessage);
        }

        var user = await userManager.Users.FirstOrDefaultAsync(user => user.Email == request.Email, cancellationToken);

        if (user is null)
        {
            throw new CustomException<ResetPasswordErrorCode>(ResetPasswordErrorCode.UserNotFound, ResetPasswordErrorMessages.UserNotFound);
        }

        bool isCurrentPasswordValid = await userManager.CheckPasswordAsync(user, request.CurrentPassword);

        if (isCurrentPasswordValid)
        {
            string resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            IdentityResult passwordChangeResult = await userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

            return Unit.Value;
        }

        throw new CustomException<ResetPasswordErrorCode>(ResetPasswordErrorCode.InvalidCurrentPassword, ResetPasswordErrorMessages.InvalidCurrentPassword);
    }
}