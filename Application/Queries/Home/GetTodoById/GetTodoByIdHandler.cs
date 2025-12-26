using Application.Dto.GetTodo;
using Application.Queries.Home.GetTodoById.Models;
using Application.Services.Http;
using AutoMapper;
using Infrastructure.Context;
using Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.Common.Exceptions;

namespace Application.Queries.Home.GetTodoById;

public class GetTodoByIdHandler(
    IHttpContextDataExtractor authUtils,
    ITaskManagementToolDbContext dbContext,
    IMapper mapper
    ) : IRequestHandler<GetTodoByIdQuery, GetTodoByIdResponse>
{
    private readonly IHttpContextDataExtractor _authUtils = authUtils;
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<GetTodoByIdResponse> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        if (!await _authUtils.IsAllowedActionAsync(request.HttpContext, request.TodoId, cancellationToken))
        {
            throw new CustomException<GetTodoByIdErrorCode>(GetTodoByIdErrorCode.Forbidden, GetTodoByIdErrorMesssages.Forbidden);
        }

        var todoEntity = await _dbContext.Todos
            .Include(todo => todo.Creator)
            .FirstOrDefaultAsync(todo => todo.Id == request.TodoId, cancellationToken);

        var result = todoEntity is null
            ? throw new CustomException<GetTodoByIdErrorCode>(GetTodoByIdErrorCode.TodoNotFound, GetTodoByIdErrorMesssages.Forbidden)
            : _mapper.Map<ToDoEntity, TodoDto>(todoEntity);

        return new GetTodoByIdResponse { Todo = result };
    }
}