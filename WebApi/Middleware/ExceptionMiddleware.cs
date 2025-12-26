using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Exceptions;
using System;
using System.Threading.Tasks;

namespace WebApi.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, ILoggerManager loggerManager)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception is CustomExceptionBase customException)
            {
                context.Response.StatusCode = customException.ErrorCode.ToString() switch
                {
                    var code when code.Contains("NotFound", StringComparison.OrdinalIgnoreCase) => StatusCodes.Status404NotFound,
                    var code when code.Contains("Forbidden", StringComparison.OrdinalIgnoreCase) => StatusCodes.Status403Forbidden,
                    var code when code.Contains("Unauthorized", StringComparison.OrdinalIgnoreCase) => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status400BadRequest
                };
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            loggerManager.LogError(exception);

            await context.Response.WriteAsync(exception switch
            {
                CustomExceptionBase ex => JsonConvert.SerializeObject(new ProblemDetails { Title = ex.ErrorCode.ToString(), Detail = ex.Message, Status = context.Response.StatusCode }),

                _ => JsonConvert.SerializeObject(new ProblemDetails { Title = "Internal server error", Detail = "Unexpected error occured", Status = context.Response.StatusCode })
            });
        }
    }
}