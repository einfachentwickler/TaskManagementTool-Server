using Application.Dto.GetTodo;

namespace Application.Commands.Home.CreateTodo.Models;

//todo fix this dto
public record CreateTodoResponse
{
    public TodoDto Todo { get; init; }
}