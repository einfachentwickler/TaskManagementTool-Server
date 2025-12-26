using Application.Dto.GetEntity;
using MediatR;

namespace Application.Queries.Admin.GetTodos.Models;

public record GetTodosByAdminQuery : GetPagedEntityRequestBase, IRequest<GetTodosByAdminResponse> { }