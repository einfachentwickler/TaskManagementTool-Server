using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Home.GetTodos.Models;
using TaskManagementTool.BusinessLogic.Commands.Utils;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Entities;

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

        IEnumerable<TodoEntry> todos = await todoRepository.GetAsync(userId, request.PageSize, request.PageNumber);

        return new GetTodosResponse { Todos = mapper.Map<IEnumerable<TodoDto>>(todos) };
    }
}