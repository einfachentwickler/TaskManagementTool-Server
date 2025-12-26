using Application.Dto.GetTodo;
using Application.Queries.Admin.GetTodos.Models;
using AutoMapper;
using Infrastructure.Context;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Admin.GetTodos;

public class GetTodosByAdminHandler(ITaskManagementToolDbContext dbContext, IMapper mapper) : IRequestHandler<GetTodosByAdminQuery, GetTodosByAdminResponse>
{
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<GetTodosByAdminResponse> Handle(GetTodosByAdminQuery request, CancellationToken cancellationToken)
    {
        var todos = await _dbContext.Todos
            .OrderByDescending(todo => todo.Importance)
            .Page(request.PageSize, request.PageNumber)
            .Include(todo => todo.Creator)
            .ToListAsync(cancellationToken);

        return new GetTodosByAdminResponse { Todos = _mapper.Map<IEnumerable<TodoDto>>(todos) };
    }
}