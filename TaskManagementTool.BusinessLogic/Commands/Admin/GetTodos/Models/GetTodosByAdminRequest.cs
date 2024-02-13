using MediatR;

namespace TaskManagementTool.BusinessLogic.Commands.Admin.GetTodos.Models;

public class GetTodosByAdminRequest : IRequest<GetTodosByAdminResponse>
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }
}