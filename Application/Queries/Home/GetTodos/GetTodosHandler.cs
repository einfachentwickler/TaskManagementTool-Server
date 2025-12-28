using Application.Queries.Home.GetTodos.Models;
using Application.Services.Http;
using Infrastructure.Context;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Home.GetTodos;

public class GetTodosHandler(
    ITaskManagementToolDbContext dbContext,
    IHttpContextDataExtractor authUtils,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetTodosQuery, GetTodosResponse>
{
    private readonly IHttpContextDataExtractor _authUtils = authUtils;
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<GetTodosResponse> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        string userId = _authUtils.GetUserNameIdentifier(_httpContextAccessor.HttpContext);

        var todos = await _dbContext.Todos
            .Where(todo => todo.CreatorId == userId)
            .OrderByDescending(todo => todo.Importance)
            .Page(request.PageSize, request.PageNumber)
            .Include(todo => todo.Creator)
            .ToListAsync(cancellationToken);

        return new GetTodosResponse
        {
            Todos = todos.Select(x => new GetTodoDto
            {
                Content = x.Content,
                Importance = x.Importance,
                Id = x.Id,
                IsCompleted = x.IsCompleted,
                Name = x.Name
            }).ToList()
        };
    }
}