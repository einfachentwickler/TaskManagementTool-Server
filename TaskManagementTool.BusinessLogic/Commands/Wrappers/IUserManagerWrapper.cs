using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Commands.Wrappers;
public interface IUserManagerWrapper
{
    public IQueryable<User> Users { get; }
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<User> FindByEmailAsync(string email);
}