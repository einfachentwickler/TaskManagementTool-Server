using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.ViewModels;

namespace TaskManagementTool.BusinessLogic.Interfaces;

public interface IAdminHandler
{
    public Task<IEnumerable<UserDto>> GetAsync(int pageNumber, int pageSize);

    public Task<UserDto> GetAsync(string id);

    public Task UpdateAsync(UserDto user);

    public Task BlockOrUnblockAsync(string userId);

    public Task DeleteAsync(string id);
}