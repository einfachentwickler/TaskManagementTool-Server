using AutoMapper;
using FluentValidation;
using Infrastructure.Context;
using Infrastructure.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Home.CreateTodo.Models;
using TaskManagementTool.BusinessLogic.Commands.Utils;
using TaskManagementTool.BusinessLogic.Dto;
using TaskManagementTool.BusinessLogic.Dto.ToDoModels;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.BusinessLogic.Commands.Home.CreateTodo;

public class CreateTodoHandler(
    IAuthUtils authUtils,
    ITaskManagementToolDbContext dbContext,
    IMapper mapper,
    IValidator<CreateTodoRequest> requestValidator
    ) : IRequestHandler<CreateTodoRequest, CreateTodoResponse>
{
    private readonly IAuthUtils _authUtils = authUtils;
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<CreateTodoRequest> _requestValidator = requestValidator;

    public async Task<CreateTodoResponse> Handle(CreateTodoRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new TaskManagementToolException(ApiErrorCode.InvalidInput, string.Join(", ", validationResult.Errors));
        }

        request.CreateTodoDto.CreatorId = _authUtils.GetUserId(request.HttpContext);

        var todoEntry = _mapper.Map<CreateTodoDto, ToDoEntity>(request.CreateTodoDto);

        var createdTodo = (await _dbContext.Todos.AddAsync(todoEntry, cancellationToken)).Entity;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateTodoResponse { Todo = _mapper.Map<ToDoEntity, TodoDto>(createdTodo) };
    }
}