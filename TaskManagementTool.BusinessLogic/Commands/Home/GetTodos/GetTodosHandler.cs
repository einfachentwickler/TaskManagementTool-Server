using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Contracts;
using Infrastructure.Data.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Home.GetTodos.Models;
using TaskManagementTool.BusinessLogic.Commands.Utils;
using TaskManagementTool.BusinessLogic.Dto;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.BusinessLogic.Commands.Home.GetTodos;

public class GetTodosHandler(
    ITodoRepository todoRepository,
    IMapper mapper,
    IAuthUtils authUtils,
    IValidator<GetTodosRequest> requestValidator) : IRequestHandler<GetTodosRequest, GetTodosResponse>
{
    public async Task<GetTodosResponse> Handle(GetTodosRequest request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new TaskManagementToolException(ApiErrorCode.InvalidInput, string.Join(", ", validationResult.Errors));
        }

        string userId = authUtils.GetUserId(request.HttpContext);

        IEnumerable<ToDoEntity> todos = await todoRepository.GetAsync(userId, request.PageSize, request.PageNumber);

        return new GetTodosResponse { Todos = mapper.Map<IEnumerable<TodoDto>>(todos) };
    }
}