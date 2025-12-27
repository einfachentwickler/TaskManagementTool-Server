using Application.Dto.GetTodo;
using Application.Queries.Home.GetTodoById.Models;
using AutoMapper;
using Infrastructure.Context;
using Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Home.GetTodoById;

public class GetTodoByIdHandler(
    ITaskManagementToolDbContext dbContext,
    IMapper mapper
    ) : IRequestHandler<GetTodoByIdQuery, GetTodoByIdResponse>
{
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<GetTodoByIdResponse> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        var todoEntity = await _dbContext.Todos
            .Include(todo => todo.Creator)
            .FirstOrDefaultAsync(todo => todo.Id == request.TodoId && todo.CreatorId == todo.Creator.Id, cancellationToken);

        var result = todoEntity is null
            ? throw new CustomException<GetTodoByIdErrorCode>(GetTodoByIdErrorCode.TodoNotFound, GetTodoByIdErrorMesssages.Forbidden)
            : _mapper.Map<ToDoEntity, TodoDto>(todoEntity);

        return new GetTodoByIdResponse { Todo = result };
    }
}