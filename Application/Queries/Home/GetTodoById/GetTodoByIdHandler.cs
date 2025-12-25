using Application.Commands.Utils;
using Application.Dto;
using Application.Queries.Home.GetTodoById.Models;
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

namespace Application.Queries.Home.GetTodoById;

public class GetTodoByIdHandler(
    IAuthUtils authUtils,
    ITaskManagementToolDbContext dbContext,
    IValidator<GetTodoByIdQuery> requestValidator,
    IMapper mapper
    ) : IRequestHandler<GetTodoByIdQuery, GetTodoByIdResponse>
{
    private readonly IAuthUtils _authUtils = authUtils;
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IValidator<GetTodoByIdQuery> _requestValidator = requestValidator;
    private readonly IMapper _mapper = mapper;

    public async Task<GetTodoByIdResponse> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new CustomException(ApiErrorCode.InvalidInput, string.Join(", ", validationResult.Errors));
        }

        if (!await _authUtils.IsAllowedActionAsync(request.HttpContext, request.TodoId, cancellationToken))
        {
            throw new CustomException(ApiErrorCode.Forbidden, "");
        }

        var todoEntity = await _dbContext.Todos
            .Include(todo => todo.Creator)
            .FirstOrDefaultAsync(todo => todo.Id == request.TodoId, cancellationToken);

        var result = todoEntity is null
            ? throw new CustomException(ApiErrorCode.TodoNotFound, $"Todo with id {request.TodoId} was not found")
            : _mapper.Map<ToDoEntity, TodoDto>(todoEntity);

        return new GetTodoByIdResponse { Todo = result };
    }
}