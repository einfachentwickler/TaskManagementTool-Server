using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementTool.BusinessLogic.Commands.Wrappers;

public interface IUserManagerWrapper
{
    public IQueryable<UserEntity> Users { get; }
    public Task<bool> CheckPasswordAsync(UserEntity user, string password);
    public Task<UserEntity> FindByEmailAsync(string email);
    public Task<IdentityResult> UpdateAsync(UserEntity user);
    public Task<IdentityResult> DeleteAsync(UserEntity user);
}