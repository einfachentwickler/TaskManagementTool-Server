using System.Collections.Generic;
using AutoMapper;
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

            CreateMap<Todo, TodoDto>();
            CreateMap<TodoDto, Todo>();

            CreateMap<CreateTodoDto, Todo>();
            CreateMap<UpdateTodoDto, Todo>();

            CreateMap<IEnumerable<Todo>, List<TodoDto>>();
            CreateMap<IEnumerable<User>, List<UserDto>>();
        }
    }
}
