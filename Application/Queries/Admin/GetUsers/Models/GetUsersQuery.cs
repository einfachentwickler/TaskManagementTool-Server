using MediatR;

namespace Application.Queries.Admin.GetUsers.Models;

public record GetUsersQuery : IRequest<GetUsersResponse>
{
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
}