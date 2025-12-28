using Application.Commands.Home.CreateTodo.Models;
using Application.Dto.GetTodo;
using Application.Services.Http;
using AutoMapper;
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
    IMapper mapper,
    IValidator<CreateTodoCommand> requestValidator,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<CreateTodoCommand, CreateTodoResponse>
{
    private readonly IHttpContextDataExtractor _authUtils = authUtils;
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;
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

        var todoEntity = _mapper.Map<CreateTodoCommand, ToDoEntity>(request);

        todoEntity.CreatorId = _authUtils.GetUserNameIdentifier(_httpContextAccessor.HttpContext);

        var createdTodo = (await _dbContext.Todos.AddAsync(todoEntity, cancellationToken)).Entity;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateTodoResponse { Todo = _mapper.Map<ToDoEntity, TodoDto>(createdTodo) };
    }
}