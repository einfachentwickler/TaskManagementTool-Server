using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Dto.Errors;
using TaskManagementTool.Common.Exceptions;

namespace TaskManagementTool.Host.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            context.Response.StatusCode = exception.GetType().Name switch
            {
                nameof(TaskManagementToolException) => (int)HttpStatusCode.BadRequest,

                _ => 500
            };

            await context.Response.WriteAsync(exception switch
            {
                TaskManagementToolException ex => JsonConvert.SerializeObject(new ErrorDto(ex.ErrorCode, ex.ErrorMessage)),

                _ => "Internal server error"
            });
        }
    }
}