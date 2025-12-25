using Application.Dto;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Application.Commands.Admin.GetTodos.Models;

public class GetTodosByAdminResponse
{
    [JsonProperty("todos")]
    public IEnumerable<TodoDto> Todos { get; init; }
}