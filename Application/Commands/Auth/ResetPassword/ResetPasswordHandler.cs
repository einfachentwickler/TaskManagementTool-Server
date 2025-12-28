using Application.Commands.Auth.ResetPassword.Models;
using Application.Services.IdentityUserManagement;
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
    IIdentityUserManagerWrapper userManager
    ) : IRequestHandler<ResetPasswordCommand, Unit>
{
    private readonly IIdentityUserManagerWrapper _userManager = userManager;
    private readonly IValidator<ResetPasswordCommand> _validator = validator;

    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var firstError = validationResult.Errors[0];
            throw new CustomException<ResetPasswordErrorCode>(Enum.Parse<ResetPasswordErrorCode>(firstError.ErrorCode), firstError.ErrorMessage);
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Email == request.Email, cancellationToken);

        if (user is null)
        {
            throw new CustomException<ResetPasswordErrorCode>(ResetPasswordErrorCode.UserNotFound, ResetPasswordErrorMessages.UserNotFound);
        }

        bool isCurrentPasswordValid = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);

        if (isCurrentPasswordValid)
        {
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var passwordChangeResult = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

            return Unit.Value;
        }

        throw new CustomException<ResetPasswordErrorCode>(ResetPasswordErrorCode.InvalidCurrentPassword, ResetPasswordErrorMessages.InvalidCurrentPassword);
    }
}