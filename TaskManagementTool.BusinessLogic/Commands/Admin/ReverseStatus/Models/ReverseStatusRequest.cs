using MediatR;

namespace TaskManagementTool.BusinessLogic.Commands.Admin.ReverseStatus.Models;

public class ReverseStatusRequest : IRequest<Unit>
{
    public string UserId { get; set; }
}