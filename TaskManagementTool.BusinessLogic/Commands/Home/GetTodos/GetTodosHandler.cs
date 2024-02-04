using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Home.GetTodos.Models;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Commands.Home.GetTodos;
public class GetTodosHandler(
    ITodoRepository todoRepository,
    IMapper mapper,
    IValidator<GetTodosRequest> requestValidator) : IRequestHandler<GetTodosRequest, GetTodosResponse>
{
    public async Task<GetTodosResponse> Handle(GetTodosRequest request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new TaskManagementToolException(ApiErrorCode.InvalidInput, string.Join(", ", validationResult.Errors));
        }

        IEnumerable<TodoEntry> todos = await todoRepository.GetAsync(request.UserId, request.PageSize, request.PageNumber);

        return new GetTodosResponse { Todos = mapper.Map<IEnumerable<TodoDto>>(todos) };
    }
}