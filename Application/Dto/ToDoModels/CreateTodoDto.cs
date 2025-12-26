namespace Application.Dto.ToDoModels;

public class CreateTodoDto
{
    public string Name { get; set; }

    public string Content { get; set; }

    public int Importance { get; set; }
}