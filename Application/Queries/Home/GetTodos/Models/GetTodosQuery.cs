using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Queries.Home.GetTodos.Models;

public record GetTodosQuery : IRequest<GetTodosResponse>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}