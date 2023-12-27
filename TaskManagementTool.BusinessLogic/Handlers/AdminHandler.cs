using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Interfaces;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Handlers;

public class AdminHandler(IMapper mapper, UserManager<User> userManager, ITodoRepository todoRepository) : IAdminHandler
{
    public async Task<IEnumerable<UserDto>> GetAsync(int pageNumber, int pageSize)
    {
        IEnumerable<User> users = await userManager.Users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> GetAsync(string id)
    {
        User user = await userManager.Users.FirstOrDefaultAsync(user => user.Id == id);

        return user is null ? throw new TaskManagementToolException(ApiErrorCode.UserNotFound, $"User with id {id} was not found") : mapper.Map<User, UserDto>(user);
    }

    public async Task UpdateAsync(UserDto user)
    {
        User entityToUpdate = await CopyAsync(user);
        IdentityResult identityResult = await userManager.UpdateAsync(entityToUpdate);

        if (!identityResult.Succeeded)
        {
            string errors = string.Join("\n", identityResult.Errors);

            throw new TaskManagementToolException($"Update failed: {errors}");
        }
    }

    public async Task BlockOrUnblockAsync(string userId)
    {
        UserDto user = await GetAsync(userId);

        user.IsBlocked = !user.IsBlocked;

        await UpdateAsync(user);
    }

    public async Task DeleteAsync(string userId)
    {
        User user = await userManager.Users.FirstOrDefaultAsync(usr => usr.Id == userId) ?? throw new TaskManagementToolException(ApiErrorCode.UserNotFound, $"User with id {userId} was not found");

        await todoRepository.DeleteAsync(todo => todo.CreatorId == userId);

        IdentityResult identityResult = await userManager.DeleteAsync(user);
        if (!identityResult.Succeeded)
        {
            string errors = string.Join("\n", identityResult.Errors);

            throw new TaskManagementToolException($"Update failed: {errors}");
        }
    }

    private async Task<User> CopyAsync(UserDto user)
    {
        User userTemp = await userManager.FindByEmailAsync(user.Email);

        userTemp.Id = user.Id;
        userTemp.Age = user.Age;
        userTemp.FirstName = user.FirstName;
        userTemp.LastName = user.LastName;
        userTemp.IsBlocked = user.IsBlocked;
        userTemp.Role = user.Role;
        userTemp.Email = user.Email;
        userTemp.UserName = user.Username;

        return userTemp;
    }
}
