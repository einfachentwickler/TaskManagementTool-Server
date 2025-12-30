using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Exceptions;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace WebApi.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, ILogger<ExceptionMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;

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

            logger.LogError(new EventId(), exception, "{Message}", exception.Message);

            await context.Response.WriteAsync(exception switch
            {
                CustomExceptionBase ex => JsonConvert.SerializeObject(new ProblemDetails { Title = ex.ErrorCode.ToString(), Detail = ex.Message, Status = context.Response.StatusCode }),

                _ => JsonConvert.SerializeObject(new ProblemDetails { Title = "Internal server error", Detail = "Unexpected error occured", Status = context.Response.StatusCode })
            });
        }
    }
}