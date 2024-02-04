using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Constants;
using TaskManagementTool.BusinessLogic.Handlers.Auth.Login.Models;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Handlers.Auth.Login;

public class UserLoginHandler(
    UserManager<User> userManager,
    IValidator<UserLoginRequest> validator,
     IConfiguration configuration
    ) : IRequestHandler<UserLoginRequest, UserLoginResponse>
{
    public async Task<UserLoginResponse> Handle(UserLoginRequest request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

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

        (string tokenAsString, JwtSecurityToken jwtSecurityToken) = GetToken(user, request);

        return new UserLoginResponse
        {
            Message = tokenAsString,
            IsSuccess = true,
            ExpirationDate = jwtSecurityToken.ValidTo
        };
    }

    private (string, JwtSecurityToken) GetToken(User user, UserLoginRequest model)
    {
        Claim[] claims = [
            new("email", model.Email),
            new(ClaimTypes.NameIdentifier, user.Id),
            new("role", user.Role)
        ];

        byte[] authKey = Encoding.UTF8.GetBytes(configuration["AuthSettings:Key"]);

        SymmetricSecurityKey key = new(authKey);

        JwtSecurityToken token = new(
            configuration["AuthSettings:Issuer"],
            configuration["AuthSettings:Audience"],
            claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

        return (tokenAsString, token);
    }
}
