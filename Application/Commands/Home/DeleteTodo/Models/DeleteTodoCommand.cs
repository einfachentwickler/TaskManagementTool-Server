using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Home.DeleteTodo.Models;

public record DeleteTodoCommand : IRequest<Unit>
{
    public int TodoId { get; init; }

    public required HttpContext HttpContext { get; init; }
}