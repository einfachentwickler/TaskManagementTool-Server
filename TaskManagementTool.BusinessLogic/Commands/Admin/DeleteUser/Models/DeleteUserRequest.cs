﻿using MediatR;

namespace TaskManagementTool.BusinessLogic.Commands.Admin.DeleteUser.Models;

public class DeleteUserRequest : IRequest<Unit>
{
    public string Email { get; set; }
}