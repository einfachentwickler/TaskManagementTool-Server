using Application.Commands.Home.DeleteTodo.Models;
using Application.Commands.Utils;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;

namespace Application.Commands.Home.DeleteTodo;

public class DeleteTodoHandler(
    IAuthUtils authUtils,
    ITaskManagementToolDbContext dbContext,
    IValidator<DeleteTodoCommand> requestValidator
    ) : IRequestHandler<DeleteTodoCommand, DeleteTodoResponse>
{
    private readonly IAuthUtils _authUtils = authUtils;
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IValidator<DeleteTodoCommand> _requestValidator = requestValidator;

    public async Task<DeleteTodoResponse> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new CustomException(ApiErrorCode.InvalidInput, string.Join(", ", validationResult.Errors));
        }

        if (!await _authUtils.IsAllowedActionAsync(request.HttpContext, request.TodoId, cancellationToken))
        {
            throw new CustomException(ApiErrorCode.Forbidden, string.Empty);
        }

        await _dbContext.Todos.Where(todo => todo.Id == request.TodoId).ExecuteDeleteAsync(cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteTodoResponse { IsSuccess = true };
    }
}