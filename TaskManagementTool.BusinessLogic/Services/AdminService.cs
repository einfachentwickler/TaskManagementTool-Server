using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Contracts;
using TaskManagementTool.BusinessLogic.ViewModels;
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

        public async Task<ICollection<UserDto>> GetUsersAsync()
        {
            ICollection<User> users = await _userManager.Users.ToListAsync();
            ICollection<UserDto> mappedUsers = _mapper.Map<ICollection<UserDto>>(users);
            return mappedUsers;
        }

        public async Task<UserDto> GetUserAsync(string id)
        {
            User singleUser = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (singleUser is null)
            {
                throw new TaskManagementToolException("Passed invalid id: user is null");
            }
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

        public async Task DeleteUserAsync(string id)
        {
            ICollection<Todo> todos = await _todoRepository.GetAsync();

            todos = todos.Where(todo => todo.Creator.Id == id).ToList();

            if (todos.Any())
            {
                foreach (Todo todo in todos)
                {
                    await _todoRepository.DeleteAsync(todo.Id);
                }
            }
            User user = await _userManager.Users.FirstOrDefaultAsync(usr => usr.Id == id);

            if (user is null)
            {
                throw new TaskManagementToolException("Passed invalid id: user is null");
            }

            IdentityResult identityResult = await _userManager.DeleteAsync(user);
            if (!identityResult.Succeeded)
            {
                throw new TaskManagementToolException("Update failed: " + string.Join("\n", identityResult.Errors));
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
