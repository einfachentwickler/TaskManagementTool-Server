using MediatR;
using Microsoft.AspNetCore.Http;
using TaskManagementTool.BusinessLogic.Dto.ToDoModels;

namespace TaskManagementTool.BusinessLogic.Commands.Home.UpdateTodo.Models;

public class UpdateTodoRequest : IRequest<UpdateTodoResponse>
{
    public UpdateTodoDto UpdateTodoDto { get; set; }

    public HttpContext HttpContext { get; set; }
}