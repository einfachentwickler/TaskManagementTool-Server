using Application.Commands.Admin.GetUsers.Models;
using Application.Commands.Wrappers;
using Application.Dto;
using AutoMapper;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Admin.GetUsers;

public class GetUsersHandler(IUserManagerWrapper userManager, IMapper mapper) : IRequestHandler<GetUsersRequest, GetUsersResponse>
{
    public async Task<GetUsersResponse> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        var users = await userManager.Users.Page(request.PageSize, request.PageNumber).ToListAsync(cancellationToken);

        return new GetUsersResponse { Users = mapper.Map<IEnumerable<UserDto>>(users) };
    }
}