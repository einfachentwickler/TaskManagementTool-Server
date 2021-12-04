using AutoMapper;
using System.Collections.Generic;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.Host.Profiles
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

            CreateMap<ICollection<Todo>, List<TodoDto>>();
            CreateMap<ICollection<User>, List<UserDto>>();
        }
    }
}
