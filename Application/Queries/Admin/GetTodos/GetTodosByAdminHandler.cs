using Application.Queries.Admin.GetTodos.Models;
using Application.Queries.Admin.GetUsers.Models;
using Infrastructure.Context;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Admin.GetTodos;

public class GetTodosByAdminHandler(ITaskManagementToolDbContext dbContext) : IRequestHandler<GetTodosByAdminQuery, GetTodosByAdminResponse>
{
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;

    public async Task<GetTodosByAdminResponse> Handle(GetTodosByAdminQuery request, CancellationToken cancellationToken)
    {
        var todos = await _dbContext.Todos
            .AsNoTracking()
            .OrderByDescending(todo => todo.Importance)
            .Page(request.PageSize, request.PageNumber)
            .Include(todo => todo.Creator)
            .ToListAsync(cancellationToken);

        return new GetTodosByAdminResponse
        {
            Todos = todos.Select(x => new TodoDtoWithUser
            {
                Id = x.Id,
                Content = x.Content,
                Creator = new GetUserDto
                {
                    Age = x.Creator.Age,
                    Email = x.Creator.Email,
                    FirstName = x.Creator.FirstName,
                    LastName = x.Creator.LastName,
                    Id = x.Creator.Id,
                    IsBlocked = x.Creator.IsBlocked,
                    Role = x.Creator.Role,
                    Username = x.Creator.UserName
                },
                Importance = x.Importance,
                IsCompleted = x.IsCompleted,
                Name = x.Name
            })
        };
    }
}