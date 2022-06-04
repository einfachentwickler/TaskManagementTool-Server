using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Services
{
    public class AdminService : IAdminService
    {
        private readonly ITodoRepository _todoRepository;

        private readonly IMapper _mapper;

        private readonly UserManager<User> _userManager;

        public AdminService(IMapper mapper, UserManager<User> manager, ITodoRepository todoRepository)
        {
            _mapper = mapper;
            _userManager = manager;
            _todoRepository = todoRepository;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            IEnumerable<User> users = await _userManager.Users.ToListAsync();
            IEnumerable<UserDto> mappedUsers = _mapper.Map<IEnumerable<UserDto>>(users);
            return mappedUsers;
        }

        public async Task<UserDto> GetUserAsync(string id)
        {
            User singleUser = await _userManager.Users.FirstAsync(user => user.Id == id);

            UserDto mappedUser = _mapper.Map<User, UserDto>(singleUser);
            return mappedUser;
        }

        public async Task UpdateUserAsync(UserDto user)
        {
            User entityToUpdate = await CopyUserAsync(user);
            IdentityResult identityResult = await _userManager.UpdateAsync(entityToUpdate);

            if (!identityResult.Succeeded)
            {
                throw new TaskManagementToolException("Update failed: " + string.Join("\n", identityResult.Errors));
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
            await DeleteUsersTodos(id);

            User user = await _userManager.Users.FirstAsync(usr => usr.Id == id);

            IdentityResult identityResult = await _userManager.DeleteAsync(user);
            if (!identityResult.Succeeded)
            {
                throw new TaskManagementToolException("Update failed: " + string.Join("\n", identityResult.Errors));
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
}
