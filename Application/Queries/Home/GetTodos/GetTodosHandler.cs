using Application.Dto.GetTodo;
using Application.Queries.Home.GetTodos.Models;
using Application.Services.Http;
using AutoMapper;
using Infrastructure.Context;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Home.GetTodos;

public class GetTodosHandler(
    ITaskManagementToolDbContext dbContext,
    IMapper mapper,
    IHttpContextDataExtractor authUtils) : IRequestHandler<GetTodosQuery, GetTodosResponse>
{
    private readonly IMapper _mapper = mapper;
    private readonly IHttpContextDataExtractor _authUtils = authUtils;
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;

    public async Task<GetTodosResponse> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        string userId = _authUtils.GetUserNameIdentifier(request.HttpContext);

        var todos = await _dbContext.Todos
            .Where(todo => todo.CreatorId == userId)
            .OrderByDescending(todo => todo.Importance)
            .Page(request.PageSize, request.PageNumber)
            .Include(todo => todo.Creator)
            .ToListAsync(cancellationToken);

        return new GetTodosResponse { Todos = _mapper.Map<IEnumerable<TodoDto>>(todos) };
    }
}