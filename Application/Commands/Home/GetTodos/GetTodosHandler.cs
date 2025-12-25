using Application.Commands.Home.GetTodos.Models;
using Application.Commands.Utils;
using Application.Dto;
using AutoMapper;
using FluentValidation;
using Infrastructure.Context;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;

namespace Application.Commands.Home.GetTodos;

public class GetTodosHandler(
    ITaskManagementToolDbContext dbContext,
    IMapper mapper,
    IAuthUtils authUtils,
    IValidator<GetTodosRequest> requestValidator) : IRequestHandler<GetTodosRequest, GetTodosResponse>
{
    private readonly IMapper _mapper = mapper;
    private readonly IAuthUtils _authUtils = authUtils;
    private readonly IValidator<GetTodosRequest> _requestValidator = requestValidator;
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;

    public async Task<GetTodosResponse> Handle(GetTodosRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new TaskManagementToolException(ApiErrorCode.InvalidInput, string.Join(", ", validationResult.Errors));
        }

        string userId = _authUtils.GetUserId(request.HttpContext);

        var todos = await _dbContext.Todos
            .OrderByDescending(todo => todo.Importance)
            .Page(request.PageSize, request.PageNumber)
            .Include(todo => todo.Creator)
            .ToListAsync(cancellationToken: cancellationToken);

        return new GetTodosResponse { Todos = _mapper.Map<IEnumerable<TodoDto>>(todos) };
    }
}