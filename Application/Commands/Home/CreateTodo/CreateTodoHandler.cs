using Application.Commands.Home.CreateTodo.Models;
using Application.Services.Http;
using FluentValidation;
using Infrastructure.Context;
using Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Home.CreateTodo;

public class CreateTodoHandler(
    IHttpContextDataExtractor authUtils,
    ITaskManagementToolDbContext dbContext,
    IValidator<CreateTodoCommand> requestValidator,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<CreateTodoCommand, CreateTodoResponse>
{
    private readonly IHttpContextDataExtractor _authUtils = authUtils;
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IValidator<CreateTodoCommand> _requestValidator = requestValidator;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<CreateTodoResponse> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var firstError = validationResult.Errors[0];
            throw new CustomException<CreateTodoErrorCode>(Enum.Parse<CreateTodoErrorCode>(firstError.ErrorCode), firstError.ErrorMessage);
        }

        var todoEntity = new ToDoEntity
        {
            Content = request.Content,
            CreatorId = _authUtils.GetUserNameIdentifier(_httpContextAccessor.HttpContext),
            Importance = request.Importance,
            IsCompleted = false,
            Name = request.Name,
        };

        var createdTodo = (await _dbContext.Todos.AddAsync(todoEntity, cancellationToken)).Entity;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateTodoResponse
        {
            Content = createdTodo.Content,
            Id = createdTodo.Id,
            Importance = createdTodo.Importance,
            IsCompleted = createdTodo.IsCompleted,
            Name = createdTodo.Name,
        };
    }
}