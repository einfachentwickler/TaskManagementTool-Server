using Application.Dto;
using MediatR;

namespace Application.Queries.Admin.GetUsers.Models;

public record GetUsersQuery : GetPagedEntityBase, IRequest<GetUsersResponse> { }