using MediatR;

namespace TaskManagementTool.BusinessLogic.Commands.Admin.GetUsers.Models;

public class GetUsersRequest : IRequest<GetUsersResponse>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}