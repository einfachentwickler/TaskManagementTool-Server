using Application.Dto;
using MediatR;

namespace Application.Queries.Admin.GetTodos.Models;

public record GetTodosByAdminQuery : GetPagedEntityBase, IRequest<GetTodosByAdminResponse> { }