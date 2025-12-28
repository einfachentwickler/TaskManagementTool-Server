using System.Collections.Generic;

namespace Application.Queries.Home.GetTodos.Models;

public record GetTodosResponse
{
    public required IEnumerable<GetTodoDto> Todos { get; init; }
}