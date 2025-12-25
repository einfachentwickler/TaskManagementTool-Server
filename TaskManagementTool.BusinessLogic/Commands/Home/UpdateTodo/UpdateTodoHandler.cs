using AutoMapper;
using FluentValidation;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Home.UpdateTodo.Models;
using TaskManagementTool.BusinessLogic.Commands.Utils;
using TaskManagementTool.BusinessLogic.Dto;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.BusinessLogic.Commands.Home.UpdateTodo;

public class UpdateTodoHandler(
    ITaskManagementToolDbContext dbContext,
    IAuthUtils authUtils,
    IValidator<UpdateTodoRequest> requestValidator,
    IMapper mapper
    ) : IRequestHandler<UpdateTodoRequest, UpdateTodoResponse>
{
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IAuthUtils _authUtils = authUtils;
    private readonly IValidator<UpdateTodoRequest> _requestValidator = requestValidator;
    private readonly IMapper _mapper = mapper;

    public async Task<UpdateTodoResponse> Handle(UpdateTodoRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new TaskManagementToolException(ApiErrorCode.InvalidInput, string.Join(", ", validationResult.Errors));
        }

        if (!await _authUtils.IsAllowedActionAsync(request.HttpContext, request.UpdateTodoDto.Id, cancellationToken))
        {
            throw new TaskManagementToolException(ApiErrorCode.Forbidden, "");
        }

        var toDo = await _dbContext.Todos
            .Include(todo => todo.Creator)
            .FirstOrDefaultAsync(todo => todo.Id == request.UpdateTodoDto.Id, cancellationToken);

        toDo.Name = request.UpdateTodoDto.Name;
        toDo.IsCompleted = request.UpdateTodoDto.IsCompleted;
        toDo.Content = request.UpdateTodoDto.Content;
        toDo.Importance = request.UpdateTodoDto.Importance;

        //todo async
        var updatedTodo = _dbContext.Todos.Update(toDo).Entity;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateTodoResponse
        {
            Todo = _mapper.Map<TodoDto>(updatedTodo)
        };
    }
}