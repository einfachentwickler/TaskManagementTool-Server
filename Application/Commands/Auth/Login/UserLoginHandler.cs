using Application.Commands.Auth.Login.Models;
using Application.Commands.Auth.Login.Validation;
using Application.Services.IdentityUserManagement;
using Application.Services.Jwt;
using FluentValidation;
using MediatR;
using Shared.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Auth.Login;

public class UserLoginHandler(
    IIdentityUserManagerWrapper userManager,
    IValidator<UserLoginCommand> requestValidator,
    IJwtSecurityTokenBuilder jwtSecurityTokenBuilder
    ) : IRequestHandler<UserLoginCommand, UserLoginResponse>
{
    private readonly IIdentityUserManagerWrapper _userManager = userManager;
    private readonly IValidator<UserLoginCommand> _requestValidator = requestValidator;
    private readonly IJwtSecurityTokenBuilder _jwtSecurityTokenBuilder = jwtSecurityTokenBuilder;

    public async Task<UserLoginResponse> Handle(UserLoginCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _requestValidator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var firstError = validationResult.Errors[0];
            throw new CustomException<UserLoginErrorCode>(Enum.Parse<UserLoginErrorCode>(firstError.ErrorCode), firstError.ErrorMessage);
        }

        var user = await _userManager.FindByEmailAsync(command.Email);
        var passwordValid = user != null && await _userManager.CheckPasswordAsync(user, command.Password);

        if (user == null || !passwordValid)
        {
            throw new CustomException<UserLoginErrorCode>(UserLoginErrorCode.InvalidCredentials, UserLoginErrorMessages.InvalidCredentials);
        }

        if (user.IsBlocked)
        {
            throw new CustomException<UserLoginErrorCode>(UserLoginErrorCode.BlockedEmail, UserLoginErrorMessages.BlockedEmail);
        }

        var buildTokenResponse = _jwtSecurityTokenBuilder.Build(user.Id, user.Role, command.Email);

        return new UserLoginResponse
        {
            Token = buildTokenResponse.Token,
            Expires = buildTokenResponse.Expires
        };
    }
}