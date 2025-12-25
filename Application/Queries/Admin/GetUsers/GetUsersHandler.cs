using Application.Commands.Wrappers;
using Application.Dto;
using Application.Queries.Admin.GetUsers.Models;
using AutoMapper;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Admin.GetUsers;

public class GetUsersHandler(IUserManagerWrapper userManager, IMapper mapper) : IRequestHandler<GetUsersQuery, GetUsersResponse>
{
    public async Task<GetUsersResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userManager.Users.Page(request.PageSize, request.PageNumber).ToListAsync(cancellationToken);

        return new GetUsersResponse { Users = mapper.Map<IEnumerable<UserDto>>(users) };
    }
}