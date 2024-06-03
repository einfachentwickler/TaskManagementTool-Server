﻿using Newtonsoft.Json;

namespace TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;

public class CreateTodoDto
{
    public string Name { get; set; }

    public string Content { get; set; }

    public int Importance { get; set; }

    [JsonIgnore]
    public string CreatorId { get; set; }
}