using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Home.DeleteTodo.Models;

public class DeleteTodoRequest : IRequest<DeleteTodoResponse>
{
    public int TodoId { get; set; }

    public HttpContext HttpContext { get; set; }
}