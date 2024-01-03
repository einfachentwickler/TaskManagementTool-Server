﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Dto.Errors;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.Host.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            if (exception is TaskManagementToolException customException)
            {
                context.Response.StatusCode = customException.ErrorCode switch
                {
                    ApiErrorCode.UserNotFound => (int)HttpStatusCode.NotFound,
                    ApiErrorCode.TodoNotFound => (int)HttpStatusCode.NotFound,
                    ApiErrorCode.Unautorized => (int)HttpStatusCode.Unauthorized,
                    ApiErrorCode.InvalidInput => (int)HttpStatusCode.BadRequest,

                    _ => 500
                };
            }

            await context.Response.WriteAsync(exception switch
            {
                TaskManagementToolException ex => JsonConvert.SerializeObject(new ErrorDto(ex.ErrorCode, ex.ErrorMessage)),

                _ => "Internal server error"
            });
        }
    }
}