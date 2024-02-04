using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Auth.Login.Models;
using TaskManagementTool.BusinessLogic.Commands.Utils.Jwt;
using TaskManagementTool.BusinessLogic.Commands.Wrappers;
using TaskManagementTool.BusinessLogic.Constants;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Commands.Auth.Login;

public class UserLoginHandler(
    IUserManagerWrapper userManager,
    IValidator<UserLoginRequest> requestValidator,
    IJwtSecurityTokenBuilder jwtSecurityTokenBuilder
    ) : IRequestHandler<UserLoginRequest, UserLoginResponse>
{
    public async Task<UserLoginResponse> Handle(UserLoginRequest request, CancellationToken cancellationToken)
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

        User user = await userManager.FindByEmailAsync(request.Email);

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