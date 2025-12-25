using Application.Commands.Auth.Register.Models;
using FluentValidation;
using Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Exceptions;

namespace Application.Commands.Auth.Register;

public class UserRegisterHandler(
    IValidator<UserRegisterCommand> validator,
    UserManager<UserEntity> userManager
    ) : IRequestHandler<UserRegisterCommand, Unit>
{
    public async Task<Unit> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var firstError = validationResult.Errors[0];
            throw new CustomException<UserRegisterErrorCode>(Enum.Parse<UserRegisterErrorCode>(firstError.ErrorCode), firstError.ErrorMessage);
        }

        UserEntity identityUser = new()
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Age = request.Age,
            Role = UserRoles.USER_ROLE
        };

        var result = await userManager.CreateAsync(identityUser, request.Password);

        if (result.Succeeded)
        {
            return Unit.Value;
        }

        throw new CustomException<UserRegisterErrorCode>(UserRegisterErrorCode.InternalServerError, UserRegisterErrorMessages.InternalServerError);
    }
}