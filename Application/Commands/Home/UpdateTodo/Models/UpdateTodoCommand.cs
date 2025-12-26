using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Home.UpdateTodo.Models;

public record UpdateTodoCommand : IRequest<UpdateTodoResponse>
{
    public required UpdateTodoDto UpdateTodoDto { get; init; }

    public required HttpContext HttpContext { get; init; }
}