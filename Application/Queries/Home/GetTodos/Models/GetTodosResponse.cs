using Application.Dto;
using System.Collections.Generic;

namespace Application.Queries.Home.GetTodos.Models;

public record GetTodosResponse
{
    public required IEnumerable<TodoDto> Todos { get; init; }
}