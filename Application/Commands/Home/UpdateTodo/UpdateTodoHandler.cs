using Application.Commands.Home.UpdateTodo.Models;
using Application.Dto.GetTodo;
using Application.Services.Http;
using AutoMapper;
using FluentValidation;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.Common.Exceptions;

namespace Application.Commands.Home.UpdateTodo;

public class UpdateTodoHandler(
    ITaskManagementToolDbContext dbContext,
    IHttpContextDataExtractor authUtils,
    IValidator<UpdateTodoCommand> requestValidator,
    IMapper mapper
    ) : IRequestHandler<UpdateTodoCommand, UpdateTodoResponse>
{
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IHttpContextDataExtractor _authUtils = authUtils;
    private readonly IValidator<UpdateTodoCommand> _requestValidator = requestValidator;
    private readonly IMapper _mapper = mapper;

    public async Task<UpdateTodoResponse> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var firstError = validationResult.Errors[0];
            throw new CustomException<UpdateTodoErrorCode>(Enum.Parse<UpdateTodoErrorCode>(firstError.ErrorCode), firstError.ErrorMessage);
        }

        if (!await _authUtils.IsAllowedActionAsync(request.HttpContext, request.UpdateTodoDto.Id, cancellationToken))
        {
            throw new CustomException<UpdateTodoErrorCode>(UpdateTodoErrorCode.Forbidden, UpdateTodoErrorMessages.Forbidden);
        }

        var toDo = await _dbContext.Todos
            //todo do i need creator here?
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