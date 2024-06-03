using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Admin.GetUsers.Models;
using TaskManagementTool.BusinessLogic.Commands.Wrappers;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.DataAccess.Entities;
using TaskManagementTool.DataAccess.Extensions;

namespace TaskManagementTool.BusinessLogic.Commands.Admin.GetUsers;

public class GetUsersHandler(IUserManagerWrapper userManager, IMapper mapper) : IRequestHandler<GetUsersRequest, GetUsersResponse>
{
    public async Task<GetUsersResponse> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<UserEntry> users = await userManager.Users.Page(request.PageSize, request.PageNumber).ToListAsync(cancellationToken);

        return new GetUsersResponse { Users = mapper.Map<IEnumerable<UserDto>>(users) };
    }
}