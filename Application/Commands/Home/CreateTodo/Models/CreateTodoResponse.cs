using Application.Dto;

namespace Application.Commands.Home.CreateTodo.Models;

public class CreateTodoResponse
{
    public TodoDto Todo { get; set; }
}