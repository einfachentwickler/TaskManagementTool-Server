using Application.Commands.Home.CreateTodo.Models;
using Application.Commands.Home.UpdateTodo.Models;
using Application.Dto.GetTodo;
using Application.Queries.Admin.GetUsers.Models;
using AutoMapper;
using Infrastructure.Entities;
using System.Collections.Generic;

namespace Application.MappingProfiles;

public class DefaultMappingProfile : Profile
{
    public DefaultMappingProfile()
    {
        CreateMap<UserEntity, GetUserDto>().ReverseMap();

        CreateMap<ToDoEntity, TodoDto>().ReverseMap();

        CreateMap<CreateTodoDto, ToDoEntity>();
        CreateMap<UpdateTodoDto, ToDoEntity>();

        CreateMap<IEnumerable<ToDoEntity>, List<TodoDto>>();
        CreateMap<IEnumerable<UserEntity>, List<GetUserDto>>();
    }
}