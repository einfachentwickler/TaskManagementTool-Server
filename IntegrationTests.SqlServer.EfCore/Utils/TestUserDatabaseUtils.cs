using IntegrationTests.SqlServer.EfCore.Configuration;
using IntegrationTests.SqlServer.EfCore.Constants;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.DataAccess.Entities;

namespace IntegrationTests.SqlServer.EfCore.Utils;

public static class TestUserDatabaseUtils
{
    public static async Task RegisterTempUserAsync(string email, bool isBlocked)
    {
        User registerUser = new()
        {
            Age = 14,
            Email = email,
            FirstName = "First name",
            LastName = "Last name",
            UserName = email,
            Role = UserRoles.USER_ROLE,
            IsBlocked = isBlocked
        };

        IdentityResult result = await TestStartup.UserManager.CreateAsync(
            registerUser,
            MockDataConstants.TEMP_USER_PASSWORD
            );

        if (!result.Succeeded)
        {
            string errors = string.Join("\n", result.Errors.Select(error => new
            {
                error.Code,
                error.Description
            }));

            throw new TaskManagementToolException($"User was not created: {errors}");
        }
    }

    public static async Task<UserDto> GetUserDtoAsync(string email)
    {
        User user = await TestStartup.UserManager.FindByEmailAsync(email);

        UserDto userToReturn = TestStartup.Mapper.Map<User, UserDto>(user);

        return userToReturn;
    }

    public static async Task<User> GetUserAsync(string email)
    {
        User user = await TestStartup.UserManager.FindByEmailAsync(email);

        return user;
    }

    public static RegisterDto GetRegisterDto(string email, bool confirmPasswordMatchesPassword)
    {
        string confirmPassword = confirmPasswordMatchesPassword ? MockDataConstants.TEMP_USER_PASSWORD : "wrong";

        return new RegisterDto
        {
            Age = 14,
            Password = MockDataConstants.TEMP_USER_PASSWORD,
            ConfirmPassword = confirmPassword,
            Email = email,
            FirstName = "First name",
            LastName = "Last name"
        };
    }
}