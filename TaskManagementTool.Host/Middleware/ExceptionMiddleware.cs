using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
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
                nameof(NotImplementedException) => (int)HttpStatusCode.NotImplemented,

                nameof(TodoNotFoundException) => (int)HttpStatusCode.BadRequest,

                nameof(UserNotFoundExpection) => (int)HttpStatusCode.BadRequest,

                _ => 500
            };

            await context.Response.WriteAsync(exception switch
            {
                TodoNotFoundException => TodoNotFoundException.ErrorCode.ToString(),

                UserNotFoundExpection => UserNotFoundExpection.ErrorCode.ToString(),

                _ => "Internal server error"
            });
        }
    }
}