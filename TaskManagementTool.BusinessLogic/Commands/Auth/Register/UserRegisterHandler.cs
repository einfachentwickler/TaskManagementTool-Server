using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Auth.Register.Models;
using TaskManagementTool.BusinessLogic.Constants;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Commands.Auth.Register;

public class UserRegisterHandler(
    IValidator<UserRegisterRequest> validator,
    UserManager<User> userManager
    ) : IRequestHandler<UserRegisterRequest, UserRegisterResponse>
{
    public async Task<UserRegisterResponse> Handle(UserRegisterRequest request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new UserRegisterResponse
            {
                Message = UserManagerResponseMessages.USER_WAS_NOT_CREATED,
                IsSuccess = false,
                Errors = validationResult.Errors.ConvertAll(identityError => identityError.ErrorCode)
            };
        }

        User identityUser = new()
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Age = request.Age,
            Role = UserRoles.USER_ROLE
        };

        IdentityResult result = await userManager.CreateAsync(identityUser, request.Password);

        if (result.Succeeded)
        {
            return new UserRegisterResponse
            {
                Message = UserManagerResponseMessages.USER_CREATED,
                IsSuccess = true
            };
        }

        return new UserRegisterResponse
        {
            Message = UserManagerResponseMessages.USER_WAS_NOT_CREATED,
            IsSuccess = false,
            Errors = result.Errors.Select(identityError => identityError.Description).ToList()
        };
    }
}