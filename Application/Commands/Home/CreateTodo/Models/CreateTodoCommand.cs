using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Home.CreateTodo.Models;

public record CreateTodoCommand : IRequest<CreateTodoResponse>
{
    public required HttpContext HttpContext { get; init; }

    public required CreateTodoDto CreateTodoDto { get; init; }
}