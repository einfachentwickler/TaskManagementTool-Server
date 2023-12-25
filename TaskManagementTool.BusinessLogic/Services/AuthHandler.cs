using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Constants;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Services;

public class AuthHandler(UserManager<User> userManager, IConfiguration configuration, IValidator<RegisterDto> validator) : IAuthHandler
{
    public async Task<UserManagerResponse> LoginUserAsync(LoginDto model)
    {
        User user = await userManager.FindByEmailAsync(model.Email);

        if (user is null)
        {
            return new UserManagerResponse
            {
                Message = UserManagerResponseMessages.USER_DOES_NOT_EXIST,
                IsSuccess = false
            };
        }

        if (user.IsBlocked)
        {
            return new UserManagerResponse
            {
                Message = UserManagerResponseMessages.BLOCKED_EMAIL,
                IsSuccess = false
            };
        }

        bool isValid = await userManager.CheckPasswordAsync(user, model.Password);
        if (!isValid)
        {
            return new UserManagerResponse
            {
                Message = UserManagerResponseMessages.INVALID_CREDENTIALS,
                IsSuccess = false
            };
        }

        (string tokenAsString, JwtSecurityToken jwtSecurityToken) = GetToken(user, model);

        return new UserManagerResponse
        {
            Message = tokenAsString,
            IsSuccess = true,
            ExpiredDate = jwtSecurityToken.ValidTo
        };
    }

    public async Task<UserManagerResponse> RegisterUserAsync(RegisterDto model)
    {
        if (model.Password != model.ConfirmPassword)
        {
            return new UserManagerResponse
            {
                Message = UserManagerResponseMessages.CONFIRM_PASSWORD_DOES_NOT_MATCH_PASSWORD
            };
        }

        User identityUser = new()
        {
            Email = model.Email,
            UserName = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Age = model.Age,
            Role = UserRoles.USER_ROLE
        };

        IdentityResult result = await userManager.CreateAsync(identityUser, model.Password);

        if (result.Succeeded)
        {
            return new UserManagerResponse
            {
                Message = UserManagerResponseMessages.USER_CREATED,
                IsSuccess = true
            };
        }

        return new UserManagerResponse
        {
            Message = UserManagerResponseMessages.USER_WAS_NOT_CREATED,
            IsSuccess = false,
            Errors = result.Errors.Select(identityError => identityError.Description)
        };
    }

    private (string, JwtSecurityToken) GetToken(User user, LoginDto model)
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