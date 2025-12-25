using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Queries.Home.GetTodoById.Models;

public record GetTodoByIdQuery : IRequest<GetTodoByIdResponse>
{
    public required HttpContext HttpContext { get; init; }

    public int TodoId { get; init; }
}