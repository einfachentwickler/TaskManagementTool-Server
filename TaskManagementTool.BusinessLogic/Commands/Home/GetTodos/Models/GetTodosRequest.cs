using MediatR;

namespace TaskManagementTool.BusinessLogic.Commands.Home.GetTodos.Models;

public class GetTodosRequest : IRequest<GetTodosResponse>
{
    public string UserId { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}