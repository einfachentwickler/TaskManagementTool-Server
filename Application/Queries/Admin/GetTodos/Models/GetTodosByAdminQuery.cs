using MediatR;

namespace Application.Queries.Admin.GetTodos.Models;

public record GetTodosByAdminQuery : IRequest<GetTodosByAdminResponse>
{
    public int PageNumber { get; init; }

    public int PageSize { get; init; }
}