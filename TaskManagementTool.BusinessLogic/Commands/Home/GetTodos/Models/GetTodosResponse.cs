using Newtonsoft.Json;
using System.Collections.Generic;
using TaskManagementTool.BusinessLogic.Dto;

namespace TaskManagementTool.BusinessLogic.Commands.Home.GetTodos.Models;

public class GetTodosResponse
{
    [JsonProperty("todos")]
    public IEnumerable<TodoDto> Todos { get; set; }
}