using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Commands.Wrappers;

public class UserManagerWrapper(UserManager<User> userManager) : IUserManagerWrapper
{
    IQueryable<User> IUserManagerWrapper.Users => userManager.Users;

    public async Task<User> FindByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IdentityResult> UpdateAsync(User user)
    {
        return await userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> DeleteAsync(User user)
    {
        return await userManager.DeleteAsync(user);
    }
}