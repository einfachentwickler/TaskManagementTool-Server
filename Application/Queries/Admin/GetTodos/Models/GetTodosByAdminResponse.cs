using Application.Dto;
using System.Collections.Generic;

namespace Application.Queries.Admin.GetTodos.Models;

public record GetTodosByAdminResponse
{
    public required IEnumerable<TodoDto> Todos { get; init; }
}