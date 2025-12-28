using Application.Queries.Home.GetTodoById.Models;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Home.GetTodoById;

public class GetTodoByIdHandler(ITaskManagementToolDbContext dbContext) : IRequestHandler<GetTodoByIdQuery, GetTodoByIdResponse>
{
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;

    public async Task<GetTodoByIdResponse> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        var todoEntity = await _dbContext.Todos
            .Include(todo => todo.Creator)
            .FirstOrDefaultAsync(todo => todo.Id == request.TodoId && todo.CreatorId == todo.Creator.Id, cancellationToken);

        if (todoEntity is null)
            throw new CustomException<GetTodoByIdErrorCode>(GetTodoByIdErrorCode.TodoNotFound, GetTodoByIdErrorMesssages.TodoNotFound);

        return new GetTodoByIdResponse
        {
            Id = todoEntity.Id,
            Content = todoEntity.Content,
            Importance = todoEntity.Importance,
            IsCompleted = todoEntity.IsCompleted,
            Name = todoEntity.Name
        };
    }
}