using Application.Commands.Auth.Login.Models;
using Application.Commands.Auth.Login.Validation;
using Application.Services.IdentityUserManagement;
using Application.Services.Jwt;
using FluentValidation;
using MediatR;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.Common.Exceptions;

namespace Application.Commands.Auth.Login;

public class UserLoginHandler(
    IIdentityUserManagerWrapper userManager,
    IValidator<UserLoginCommand> requestValidator,
    IJwtSecurityTokenBuilder jwtSecurityTokenBuilder
    ) : IRequestHandler<UserLoginCommand, UserLoginResponse>
{
    public async Task<UserLoginResponse> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var firstError = validationResult.Errors[0];
            throw new CustomException<UserLoginErrorCode>(Enum.Parse<UserLoginErrorCode>(firstError.ErrorCode), firstError.ErrorMessage);
        }

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new CustomException<UserLoginErrorCode>(UserLoginErrorCode.UserNotFound, UserLoginErrorMessages.UserNotFound);
        }

        if (user.IsBlocked)
        {
            throw new CustomException<UserLoginErrorCode>(UserLoginErrorCode.BlockedEmail, UserLoginErrorMessages.BlockedEmail);
        }

        var isValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isValid)
        {
            throw new CustomException<UserLoginErrorCode>(UserLoginErrorCode.InvalidCredentials, UserLoginErrorMessages.InvalidCredentials);
        }

        (string tokenAsString, JwtSecurityToken jwtSecurityToken) = jwtSecurityTokenBuilder.Build(user, request);

        return new UserLoginResponse
        {
            Token = tokenAsString,
            ExpirationDate = jwtSecurityToken.ValidTo
        };
    }
}