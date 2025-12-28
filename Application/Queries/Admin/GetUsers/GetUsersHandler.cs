using Application.Queries.Admin.GetUsers.Models;
using Application.Services.IdentityUserManagement;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Admin.GetUsers;

public class GetUsersHandler(IIdentityUserManagerWrapper userManager) : IRequestHandler<GetUsersQuery, GetUsersResponse>
{
    private readonly IIdentityUserManagerWrapper _identityUserManager = userManager;

    public async Task<GetUsersResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _identityUserManager.Users
            .AsNoTracking()
            .Page(request.PageSize, request.PageNumber)
            .ToListAsync(cancellationToken);

        return new GetUsersResponse
        {
            Users = users.Select(x => new GetUserDto
            {
                Age = x.Age,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Id = x.Id,
                IsBlocked = x.IsBlocked,
                Role = x.Role,
                Username = x.UserName
            })
        };
    }
}