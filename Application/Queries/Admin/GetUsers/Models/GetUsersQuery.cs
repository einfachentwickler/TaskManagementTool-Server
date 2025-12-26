using Application.Dto.GetEntity;
using MediatR;

namespace Application.Queries.Admin.GetUsers.Models;

public record GetUsersQuery : GetPagedEntityRequestBase, IRequest<GetUsersResponse> { }