using Application.Commands.Home.DeleteTodo.Models;
using Application.Commands.Utils;
using FluentValidation;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.Common.Exceptions;

namespace Application.Commands.Home.DeleteTodo;

public class DeleteTodoHandler(
    IAuthUtils authUtils,
    ITaskManagementToolDbContext dbContext
    ) : IRequestHandler<DeleteTodoCommand, Unit>
{
    private readonly IAuthUtils _authUtils = authUtils;
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;

    public async Task<Unit> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        if (!await _authUtils.IsAllowedActionAsync(request.HttpContext, request.TodoId, cancellationToken))
        {
            throw new CustomException<DeleteTodoErrorCode>(DeleteTodoErrorCode.Forbidden, DeleteTodoErrorMessages.Forbidden);
        }

        await _dbContext.Todos.Where(todo => todo.Id == request.TodoId).ExecuteDeleteAsync(cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}