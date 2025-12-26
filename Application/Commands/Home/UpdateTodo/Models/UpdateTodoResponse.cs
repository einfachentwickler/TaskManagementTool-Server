using Application.Dto.GetTodo;

namespace Application.Commands.Home.UpdateTodo.Models;

public record UpdateTodoResponse
{
    public required TodoDto Todo { get; init; }
}