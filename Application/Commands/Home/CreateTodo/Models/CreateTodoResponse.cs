using Application.Dto.GetTodo;

namespace Application.Commands.Home.CreateTodo.Models;

public record CreateTodoResponse
{
    public TodoDto Todo { get; init; }
}