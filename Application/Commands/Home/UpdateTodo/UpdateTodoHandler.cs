using Application.Commands.Home.UpdateTodo.Models;
using Application.Services.Http;
using FluentValidation;
using Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Home.UpdateTodo;

public class UpdateTodoHandler(
    ITaskManagementToolDbContext dbContext,
    IHttpContextDataExtractor authUtils,
    IValidator<UpdateTodoCommand> requestValidator,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<UpdateTodoCommand, UpdateTodoResponse>
{
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IHttpContextDataExtractor _authUtils = authUtils;
    private readonly IValidator<UpdateTodoCommand> _requestValidator = requestValidator;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<UpdateTodoResponse> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var firstError = validationResult.Errors[0];
            throw new CustomException<UpdateTodoErrorCode>(Enum.Parse<UpdateTodoErrorCode>(firstError.ErrorCode), firstError.ErrorMessage);
        }

        if (!await _authUtils.IsAllowedActionAsync(_httpContextAccessor.HttpContext, request.Id, cancellationToken))
        {
            throw new CustomException<UpdateTodoErrorCode>(UpdateTodoErrorCode.Forbidden, UpdateTodoErrorMessages.Forbidden);
        }

        var todoEntity = await _dbContext.Todos.FirstOrDefaultAsync(todo => todo.Id == request.Id, cancellationToken);

        todoEntity.Name = request.Name;
        todoEntity.IsCompleted = request.IsCompleted;
        todoEntity.Content = request.Content;
        todoEntity.Importance = request.Importance;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateTodoResponse
        {
            Content = todoEntity.Content,
            Id = todoEntity.Id,
            Importance = todoEntity.Importance,
            IsCompleted = todoEntity.IsCompleted,
            Name = todoEntity.Name,
        };
    }
}