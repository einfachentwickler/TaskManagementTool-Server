using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Home.DeleteTodo.Models;

public record DeleteTodoCommand : IRequest<DeleteTodoResponse>
{
    public int TodoId { get; init; }

    public required HttpContext HttpContext { get; init; }
}