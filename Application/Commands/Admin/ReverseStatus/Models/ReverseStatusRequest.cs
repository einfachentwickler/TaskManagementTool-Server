using MediatR;

namespace Application.Commands.Admin.ReverseStatus.Models;

public class ReverseStatusRequest : IRequest<Unit>
{
    public string UserId { get; set; }
}