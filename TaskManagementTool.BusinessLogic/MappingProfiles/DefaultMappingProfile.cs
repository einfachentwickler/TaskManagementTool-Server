using AutoMapper;
using Infrastructure.Data.Entities;
using System.Collections.Generic;
using TaskManagementTool.BusinessLogic.Dto;
using TaskManagementTool.BusinessLogic.Dto.ToDoModels;

namespace TaskManagementTool.BusinessLogic.MappingProfiles;

public class DefaultMappingProfile : Profile
{
    public DefaultMappingProfile()
    {
        CreateMap<UserEntity, UserDto>().ReverseMap();

        CreateMap<ToDoEntity, TodoDto>().ReverseMap();

        CreateMap<CreateTodoDto, ToDoEntity>();
        CreateMap<UpdateTodoDto, ToDoEntity>();

        CreateMap<IEnumerable<ToDoEntity>, List<TodoDto>>();
        CreateMap<IEnumerable<UserEntity>, List<UserDto>>();
    }
}