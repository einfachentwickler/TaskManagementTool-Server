using Application.Commands.Home.GetTodoById.Models;
using Application.Commands.Utils;
using Application.Dto;
using AutoMapper;
using FluentValidation;
using Infrastructure.Context;
using Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;

namespace Application.Commands.Home.GetTodoById;

public class GetTodoByIdHandler(
    IAuthUtils authUtils,
    ITaskManagementToolDbContext dbContext,
    IValidator<GetTodoByIdRequest> requestValidator,
    IMapper mapper
    ) : IRequestHandler<GetTodoByIdRequest, GetTodoByIdResponse>
{
    private readonly IAuthUtils _authUtils = authUtils;
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IValidator<GetTodoByIdRequest> _requestValidator = requestValidator;
    private readonly IMapper _mapper = mapper;

    public async Task<GetTodoByIdResponse> Handle(GetTodoByIdRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new TaskManagementToolException(ApiErrorCode.InvalidInput, string.Join(", ", validationResult.Errors));
        }

        if (!await _authUtils.IsAllowedActionAsync(request.HttpContext, request.TodoId, cancellationToken))
        {
            throw new TaskManagementToolException(ApiErrorCode.Forbidden, "");
        }

        var todoEntity = await _dbContext.Todos
            .Include(todo => todo.Creator)
            .FirstOrDefaultAsync(todo => todo.Id == request.TodoId, cancellationToken);

        var result = todoEntity is null
            ? throw new TaskManagementToolException(ApiErrorCode.TodoNotFound, $"Todo with id {request.TodoId} was not found")
            : _mapper.Map<ToDoEntity, TodoDto>(todoEntity);

        return new GetTodoByIdResponse { Todo = result };
    }
}