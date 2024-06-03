using AutoMapper;
using System.Collections.Generic;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.MappingProfiles;
public class DefaultMappingProfile : Profile
{
    public DefaultMappingProfile()
    {
        CreateMap<UserEntry, UserDto>().ReverseMap();

        CreateMap<TodoEntry, TodoDto>().ReverseMap();

        CreateMap<CreateTodoDto, TodoEntry>();
        CreateMap<UpdateTodoDto, TodoEntry>();

        CreateMap<IEnumerable<TodoEntry>, List<TodoDto>>();
        CreateMap<IEnumerable<UserEntry>, List<UserDto>>();
    }
}