using Application.Dto.ToDoModels;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Home.UpdateTodo.Models;

public class UpdateTodoRequest : IRequest<UpdateTodoResponse>
{
    public UpdateTodoDto UpdateTodoDto { get; set; }

    public HttpContext HttpContext { get; set; }
}