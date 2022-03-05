using System.Linq;
using System.Threading.Tasks;
using IntegrationTests.SqlServer.EfCore.Configuration;
using IntegrationTests.SqlServer.EfCore.Constants;
using Microsoft.AspNetCore.Identity;
using TaskManagementTool.Common.Constants;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.DataAccess.Entities;

namespace IntegrationTests.SqlServer.EfCore.Utils
{
    public static class TestUserDatabaseUtils
    {
        public static async Task CleanupDatabase(string email)
        {
            User user = await TestStartup.UserManager.FindByEmailAsync(email);

            await TestStartup.UserManager.DeleteAsync(user);
        }

        public static async Task RegisterTempUserAsync(string email, bool isBlocked = false)
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

            IdentityResult result = await TestStartup.UserManager.CreateAsync(registerUser, MockDataConstants.TEMP_USER_PASSWORD);

            if (!result.Succeeded)
            {
                throw new TaskManagementToolException(
                    $"User was not created: {string.Join("\n", result.Errors.Select(error => new { error.Code, error.Description }))}"
                );
            }
        }
    }
}
