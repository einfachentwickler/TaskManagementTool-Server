using Application.Dto.Errors;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TaskManagementTool.Common.Enums;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.Host.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ILoggerManager loggerManager)
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
                    ApiErrorCode.UserNotFound => StatusCodes.Status404NotFound,
                    ApiErrorCode.TodoNotFound => StatusCodes.Status404NotFound,
                    ApiErrorCode.Unautorized => StatusCodes.Status401Unauthorized,
                    ApiErrorCode.InvalidInput => StatusCodes.Status400BadRequest,
                    ApiErrorCode.Forbidden => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                };
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            loggerManager.LogError(exception);

            await context.Response.WriteAsync(exception switch
            {
                TaskManagementToolException ex => JsonConvert.SerializeObject(new ErrorDto(ex.ErrorCode, ex.ErrorMessage)),

                _ => "Internal server error"
            });
        }
    }
}