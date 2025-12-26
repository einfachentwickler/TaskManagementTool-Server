using Application.Dto.GetTodo;

namespace Application.Queries.Home.GetTodoById.Models;

public record GetTodoByIdResponse
{
    public required TodoDto Todo { get; init; }
}