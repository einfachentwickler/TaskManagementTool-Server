using AutoMapper;
using System.Collections.Generic;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.Host.Configuration.Profiles
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<TodoEntry, TodoDto>();
            CreateMap<TodoDto, TodoEntry>();

            CreateMap<CreateTodoDto, TodoEntry>();
            CreateMap<UpdateTodoDto, TodoEntry>();

            CreateMap<IEnumerable<TodoEntry>, List<TodoDto>>();
            CreateMap<IEnumerable<User>, List<UserDto>>();
        }
    }
}
