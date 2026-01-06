using Application.Commands.Auth.Register.Models;
using FluentValidation;
using Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Constants;
using Shared.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

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

        var userByEmail = await userManager.FindByEmailAsync(request.Email);

        if (userByEmail is not null)
        {
            throw new CustomException<UserRegisterErrorCode>(UserRegisterErrorCode.UserAlreadyExists, UserRegisterErrorMessages.UserAlreadyExists);
        }

        var identityUser = new UserEntity
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