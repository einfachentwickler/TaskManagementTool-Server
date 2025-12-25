using Application.Commands.Home.CreateTodo.Models;
using Application.Commands.Utils;
using Application.Dto;
using Application.Dto.ToDoModels;
using AutoMapper;
using FluentValidation;
using Infrastructure.Context;
using Infrastructure.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.Common.Exceptions;

namespace Application.Commands.Home.CreateTodo;

public class CreateTodoHandler(
    IAuthUtils authUtils,
    ITaskManagementToolDbContext dbContext,
    IMapper mapper,
    IValidator<CreateTodoCommand> requestValidator
    ) : IRequestHandler<CreateTodoCommand, CreateTodoResponse>
{
    private readonly IAuthUtils _authUtils = authUtils;
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<CreateTodoCommand> _requestValidator = requestValidator;

    public async Task<CreateTodoResponse> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var firstError = validationResult.Errors[0];
            throw new CustomException<CreateTodoErrorCode>(Enum.Parse<CreateTodoErrorCode>(firstError.ErrorCode), firstError.ErrorMessage);
        }

        request.CreateTodoDto.CreatorId = _authUtils.GetUserId(request.HttpContext);

        var todoEntry = _mapper.Map<CreateTodoDto, ToDoEntity>(request.CreateTodoDto);

        var createdTodo = (await _dbContext.Todos.AddAsync(todoEntry, cancellationToken)).Entity;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateTodoResponse { Todo = _mapper.Map<ToDoEntity, TodoDto>(createdTodo) };
    }
}