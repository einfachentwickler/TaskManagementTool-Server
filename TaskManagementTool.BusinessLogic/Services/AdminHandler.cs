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

namespace TaskManagementTool.BusinessLogic.Services;

public class AdminHandler(IMapper mapper, UserManager<User> manager, ITodoRepository todoRepository) : IAdminHandler
{
    private readonly ITodoRepository _todoRepository = todoRepository;

    private readonly IMapper _mapper = mapper;

    private readonly UserManager<User> _userManager = manager;

    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        IEnumerable<User> users = await _userManager.Users.ToListAsync();

        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> GetUserAsync(string id)
    {
        User user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);

        return user is null ? throw new TaskManagementToolException(ApiErrorCode.UserNotFound, $"User with id {id} was not found") : _mapper.Map<User, UserDto>(user);
    }

    public async Task UpdateUserAsync(UserDto user)
    {
        User entityToUpdate = await CopyUserAsync(user);
        IdentityResult identityResult = await _userManager.UpdateAsync(entityToUpdate);

        if (!identityResult.Succeeded)
        {
            string errors = string.Join("\n", identityResult.Errors);

            throw new TaskManagementToolException($"Update failed: {errors}");
        }
    }

    public async Task BlockOrUnblockUserAsync(string userId)
    {
        UserDto user = await GetUserAsync(userId);

        user.IsBlocked = !user.IsBlocked;

        await UpdateUserAsync(user);
    }

    public async Task DeleteUserAsync(string id)
    {
        User user = await _userManager.Users.FirstOrDefaultAsync(usr => usr.Id == id) ?? throw new TaskManagementToolException(ApiErrorCode.UserNotFound, $"User with id {id} was not found");

        await DeleteUsersTodos(id);

        IdentityResult identityResult = await _userManager.DeleteAsync(user);
        if (!identityResult.Succeeded)
        {
            string errors = string.Join("\n", identityResult.Errors);

            throw new TaskManagementToolException($"Update failed: {errors}");
        }
    }

    private async Task DeleteUsersTodos(string id)
    {
        IEnumerable<TodoEntry> todos = (await _todoRepository.GetAsync(SearchCriteriaEnum.GetAll))
            .Where(todo => todo.Creator?.Id == id)
            .ToList();

        if (todos.Any())
        {
            foreach (TodoEntry todo in todos)
            {
                await _todoRepository.DeleteAsync(todo.Id);
            }
        }
    }

    private async Task<User> CopyUserAsync(UserDto user)
    {
        User userTemp = await _userManager.FindByEmailAsync(user.Email);

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
