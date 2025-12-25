using Application.Dto;

namespace Application.Queries.Home.GetTodoById.Models;

public record GetTodoByIdResponse
{
    public required TodoDto Todo { get; init; }
}