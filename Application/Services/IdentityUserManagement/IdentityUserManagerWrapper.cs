using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.IdentityUserManagement;

public class IdentityUserManagerWrapper(UserManager<UserEntity> userManager) : IIdentityUserManagerWrapper
{
    IQueryable<UserEntity> IIdentityUserManagerWrapper.Users => userManager.Users;

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