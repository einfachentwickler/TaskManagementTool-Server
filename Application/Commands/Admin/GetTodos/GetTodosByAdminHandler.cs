using Application.Commands.Admin.GetTodos.Models;
using Application.Dto;
using AutoMapper;
using Infrastructure.Context;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Admin.GetTodos;

public class GetTodosByAdminHandler(ITaskManagementToolDbContext dbContext, IMapper mapper) : IRequestHandler<GetTodosByAdminRequest, GetTodosByAdminResponse>
{
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<GetTodosByAdminResponse> Handle(GetTodosByAdminRequest request, CancellationToken cancellationToken)
    {
        var todos = await _dbContext.Todos
            .OrderByDescending(todo => todo.Importance)
            .Page(request.PageSize, request.PageNumber)
            .Include(todo => todo.Creator)
            .ToListAsync(cancellationToken);

        return new GetTodosByAdminResponse { Todos = _mapper.Map<IEnumerable<TodoDto>>(todos) };
    }
}