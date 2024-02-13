using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Commands.Wrappers;
public interface IUserManagerWrapper
{
    public IQueryable<User> Users { get; }
    public Task<bool> CheckPasswordAsync(User user, string password);
    public Task<User> FindByEmailAsync(string email);
    public Task<IdentityResult> UpdateAsync(User user);
}