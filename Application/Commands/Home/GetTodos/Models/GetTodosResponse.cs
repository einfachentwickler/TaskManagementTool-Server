using Application.Dto;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Application.Commands.Home.GetTodos.Models;

public class GetTodosResponse
{
    [JsonProperty("todos")]
    public IEnumerable<TodoDto> Todos { get; set; }
}