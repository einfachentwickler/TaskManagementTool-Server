using Application.Dto;

namespace Application.Commands.Home.GetTodoById.Models;

public class GetTodoByIdResponse
{
    public TodoDto Todo { get; set; }
}