using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Commands.Wrappers;

public class UserManagerWrapper(UserManager<UserEntry> userManager) : IUserManagerWrapper
{
    IQueryable<UserEntry> IUserManagerWrapper.Users => userManager.Users;

    public async Task<UserEntry> FindByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<bool> CheckPasswordAsync(UserEntry user, string password)
    {
        return await userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IdentityResult> UpdateAsync(UserEntry user)
    {
        return await userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> DeleteAsync(UserEntry user)
    {
        return await userManager.DeleteAsync(user);
    }
}