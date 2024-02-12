using MediatR;
using Microsoft.AspNetCore.Http;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;

namespace TaskManagementTool.BusinessLogic.Commands.Home.CreateTodo.Models;

public class CreateTodoRequest : IRequest<CreateTodoResponse>
{
    public HttpContext HttpContext { get; set; }

    public CreateTodoDto CreateTodoDto { get; set; }
}