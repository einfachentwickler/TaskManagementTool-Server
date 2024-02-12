using MediatR;
using Microsoft.AspNetCore.Http;

namespace TaskManagementTool.BusinessLogic.Commands.Home.GetTodoById.Models;

public class GetTodoByIdRequest : IRequest<GetTodoByIdResponse>
{
    public HttpContext HttpContext { get; set; }

    public int TodoId { get; set; }
}