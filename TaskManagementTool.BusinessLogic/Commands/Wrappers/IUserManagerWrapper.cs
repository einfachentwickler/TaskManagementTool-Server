using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Commands.Wrappers;
public interface IUserManagerWrapper
{
    public IQueryable<UserEntry> Users { get; }
    public Task<bool> CheckPasswordAsync(UserEntry user, string password);
    public Task<UserEntry> FindByEmailAsync(string email);
    public Task<IdentityResult> UpdateAsync(UserEntry user);
    public Task<IdentityResult> DeleteAsync(UserEntry user);
}