using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.ViewModels;

namespace TaskManagementTool.BusinessLogic.Contracts
{
    public interface IAdminService
    {
        public Task<IEnumerable<UserDto>> GetUsersAsync();

        public Task<UserDto> GetUserAsync(string id);

        public Task UpdateUserAsync(UserDto user);
        
        public Task BlockOrUnblockUserAsync(string userId);

        public Task DeleteUserAsync(string id);
    }
}
