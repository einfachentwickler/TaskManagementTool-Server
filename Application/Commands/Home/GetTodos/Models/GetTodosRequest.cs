using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Home.GetTodos.Models;

public class GetTodosRequest : IRequest<GetTodosResponse>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public HttpContext HttpContext { get; set; }
}