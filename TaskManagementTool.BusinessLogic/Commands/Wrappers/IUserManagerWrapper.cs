using System.Threading.Tasks;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Commands.Wrappers;
public interface IUserManagerWrapper
{
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<User> FindByEmailAsync(string email);
}