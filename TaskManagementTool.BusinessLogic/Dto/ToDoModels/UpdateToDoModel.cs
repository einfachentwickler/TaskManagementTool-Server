namespace TaskManagementTool.BusinessLogic.Dto.ToDoModels;

public class UpdateTodoDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Content { get; init; }
    public bool IsCompleted { get; set; }
    public int Importance { get; set; }
}