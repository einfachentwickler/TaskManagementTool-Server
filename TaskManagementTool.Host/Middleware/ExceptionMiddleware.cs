using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace TaskManagementTool.Host.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, ILogger<ExceptionMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                context.Response.StatusCode = exception.GetType().Name switch
                {
                    nameof(NotImplementedException) => 501,
                    _ => 500
                };
                logger.LogError(JsonConvert.SerializeObject(exception));
                await context.Response.WriteAsync("Error happened...");
            }
        }
    }
}
