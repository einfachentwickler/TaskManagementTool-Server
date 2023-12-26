﻿using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TaskManagementTool.BusinessLogic.Handlers.Utils;

public interface IAuthUtils
{
    public string GetUserId(HttpContext context);

    public Task<bool> IsAllowedAction(HttpContext context, int todoId);
}