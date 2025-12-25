namespace TaskManagementTool.BusinessLogic.Dto;

public class TodoDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Content { get; set; }

    public bool IsCompleted { get; set; }

    public int Importance { get; set; }

    public UserDto Creator { get; set; }
}