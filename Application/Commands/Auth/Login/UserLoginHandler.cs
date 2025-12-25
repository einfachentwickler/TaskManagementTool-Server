using Application.Commands.Auth.Login.Models;
using Application.Commands.Utils.Jwt;
using Application.Commands.Wrappers;
using Application.Constants;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Entities;
using MediatR;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Auth.Login;

public class UserLoginHandler(
    IUserManagerWrapper userManager,
    IValidator<UserLoginCommand> requestValidator,
    IJwtSecurityTokenBuilder jwtSecurityTokenBuilder
    ) : IRequestHandler<UserLoginCommand, UserLoginResponse>
{
    public async Task<UserLoginResponse> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new UserLoginResponse
            {
                Message = UserManagerResponseMessages.USER_WAS_NOT_CREATED,
                IsSuccess = false,
                Errors = validationResult.Errors.ConvertAll(identityError => identityError.ErrorCode)
            };
        }

        UserEntity user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return new UserLoginResponse
            {
                Message = UserManagerResponseMessages.USER_DOES_NOT_EXIST,
                IsSuccess = false
            };
        }

        if (user.IsBlocked)
        {
            return new UserLoginResponse
            {
                Message = UserManagerResponseMessages.BLOCKED_EMAIL,
                IsSuccess = false
            };
        }

        bool isValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isValid)
        {
            return new UserLoginResponse
            {
                Message = UserManagerResponseMessages.INVALID_CREDENTIALS,
                IsSuccess = false
            };
        }

        (string tokenAsString, JwtSecurityToken jwtSecurityToken) = jwtSecurityTokenBuilder.Build(user, request);

        return new UserLoginResponse
        {
            Message = tokenAsString,
            IsSuccess = true,
            ExpirationDate = jwtSecurityToken.ValidTo
        };
    }
}