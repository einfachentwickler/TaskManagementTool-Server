using Newtonsoft.Json;
using System.Collections.Generic;
using TaskManagementTool.BusinessLogic.ViewModels;

namespace TaskManagementTool.BusinessLogic.Commands.Admin.GetTodos.Models;

public class GetTodosByAdminResponse
{
    [JsonProperty("todos")]
    public IEnumerable<TodoDto> Todos { get; init; }
}