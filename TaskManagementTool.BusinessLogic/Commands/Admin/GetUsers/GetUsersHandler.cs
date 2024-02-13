using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Admin.GetUsers.Models;
using TaskManagementTool.BusinessLogic.Commands.Wrappers;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;
using TaskManagementTool.DataAccess.Entities;
using TaskManagementTool.DataAccess.Extensions;

namespace TaskManagementTool.BusinessLogic.Commands.Admin.GetUsers;

public class GetUsersHandler(
    IValidator<GetUsersRequest> requestValidator,
    IUserManagerWrapper userManager,
    IMapper mapper
    ) : IRequestHandler<GetUsersRequest, GetUsersResponse>
{
    public async Task<GetUsersResponse> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new TaskManagementToolException(ApiErrorCode.InvalidInput, string.Join(", ", validationResult.Errors));
        }

        IEnumerable<User> users = await userManager.Users.Page(request.PageSize, request.PageNumber).ToListAsync(cancellationToken);

        return new GetUsersResponse { Users = mapper.Map<IEnumerable<UserDto>>(users) };
    }
}