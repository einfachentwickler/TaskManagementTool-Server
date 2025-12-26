using System.Collections.Generic;

namespace Application.Queries.Admin.GetUsers.Models;

public record GetUsersResponse
{
    public required IEnumerable<GetUserDto> Users { get; init; }
}