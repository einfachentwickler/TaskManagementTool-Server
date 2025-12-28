using System.Collections.Generic;

namespace Application.Queries.Admin.GetTodos.Models;

public record GetTodosByAdminResponse
{
    public required IEnumerable<TodoDtoWithUser> Todos { get; init; }
}