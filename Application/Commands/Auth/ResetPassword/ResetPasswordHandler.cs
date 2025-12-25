using Application.Commands.Auth.ResetPassword.Models;
using Application.Constants;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Auth.ResetPassword;

public class ResetPasswordHandler(
    IValidator<ResetPasswordCommand> validator,
    UserManager<UserEntity> userManager
    ) : IRequestHandler<ResetPasswordCommand, ResetPasswordResponse>
{
    public async Task<ResetPasswordResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ResetPasswordResponse
            {
                Message = UserManagerResponseMessages.USER_WAS_NOT_CREATED,
                IsSuccess = false,
                Errors = validationResult.Errors.ConvertAll(identityError => identityError.ErrorCode)
            };
        }

        UserEntity user = await userManager.Users.FirstOrDefaultAsync(user => user.Email == request.Email, cancellationToken);

        if (user is null)
        {
            return new ResetPasswordResponse
            {
                IsSuccess = false,
                Message = UserManagerResponseMessages.USER_DOES_NOT_EXIST
            };
        }

        bool isCurrentPasswordValid = await userManager.CheckPasswordAsync(user, request.CurrentPassword);

        if (isCurrentPasswordValid)
        {
            string resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            IdentityResult passwordChangeResult = await userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

            return new ResetPasswordResponse { IsSuccess = true };
        }

        return new ResetPasswordResponse { IsSuccess = false, Message = UserManagerResponseMessages.INVALID_CREDENTIALS };
    }
}