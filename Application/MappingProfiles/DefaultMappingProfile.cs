using Application.Dto;
using Application.Dto.ToDoModels;
using AutoMapper;
using Infrastructure.Entities;
using System.Collections.Generic;

namespace Application.MappingProfiles;

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