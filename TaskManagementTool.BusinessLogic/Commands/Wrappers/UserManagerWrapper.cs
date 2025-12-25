using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementTool.BusinessLogic.Commands.Wrappers;

public class UserManagerWrapper(UserManager<UserEntity> userManager) : IUserManagerWrapper
{
    IQueryable<UserEntity> IUserManagerWrapper.Users => userManager.Users;

    public async Task<UserEntity> FindByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<bool> CheckPasswordAsync(UserEntity user, string password)
    {
        return await userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IdentityResult> UpdateAsync(UserEntity user)
    {
        return await userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> DeleteAsync(UserEntity user)
    {
        return await userManager.DeleteAsync(user);
    }
}