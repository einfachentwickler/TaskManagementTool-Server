using Application.Dto.ToDoModels;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Home.CreateTodo.Models;

public class CreateTodoRequest : IRequest<CreateTodoResponse>
{
    public HttpContext HttpContext { get; set; }

    public CreateTodoDto CreateTodoDto { get; set; }
}