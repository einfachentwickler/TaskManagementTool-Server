using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Contracts;
using Infrastructure.Data.Entities;
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
    ITodoRepository todoRepository,
    IMapper mapper,
    IValidator<CreateTodoRequest> requestValidator
    ) : IRequestHandler<CreateTodoRequest, CreateTodoResponse>
{
    public async Task<CreateTodoResponse> Handle(CreateTodoRequest request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new TaskManagementToolException(ApiErrorCode.InvalidInput, string.Join(", ", validationResult.Errors));
        }

        request.CreateTodoDto.CreatorId = authUtils.GetUserId(request.HttpContext);

        ToDoEntity todoEntry = mapper.Map<CreateTodoDto, ToDoEntity>(request.CreateTodoDto);

        return new CreateTodoResponse { Todo = mapper.Map<ToDoEntity, TodoDto>(await todoRepository.CreateAsync(todoEntry)) };
    }
}