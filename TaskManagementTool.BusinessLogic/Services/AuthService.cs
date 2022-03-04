using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;

        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> managerPar, IConfiguration configuration)
            => (_userManager, _configuration) = (managerPar, configuration);

        public async Task<UserManagerResponse> LoginUserAsync(LoginDto model)
        {
            User user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with this email",
                    IsSuccess = false
                };
            }

            if (user.IsBlocked)
            {
                return new UserManagerResponse
                {
                    Message = "This email was blocked",
                    IsSuccess = false
                };
            }

            bool result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
            {
                return new UserManagerResponse
                {
                    Message = "Incorrect login or password",
                    IsSuccess = false
                };
            }

            Claim[] claims = {
                new("email",model.Email),
                new(ClaimTypes.NameIdentifier,user.Id),
                new("role",user.Role)
            };

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            JwtSecurityToken token = new(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message = tokenAsString,
                IsSuccess = true,
                ExpiredDate = token.ValidTo
            };
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterDto model)
        {
            if (model is null)
            {
                throw new TaskManagementToolException("Register model is null");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the password"
                };
            }

            User identityUser = new()
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.Age,
                Role = "User"
            };

            IdentityResult result = await _userManager.CreateAsync(identityUser, model.Password);

            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "User successfully created",
                    IsSuccess = true
                };
            }

            return new UserManagerResponse
            {
                Message = "User was not created",
                IsSuccess = false,
                Errors = result.Errors.Select(identityError => identityError.Description)
            };
        }
    }
}
