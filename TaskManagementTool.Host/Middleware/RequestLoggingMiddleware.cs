using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TaskManagementTool.Host.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, ILogger<ExceptionMiddleware> logger)
        {
            string url = context.Request.GetDisplayUrl();
            string method = context.Request.Method;
            logger.LogInformation($"{method} request was sent to {url}");
            await _next(context);
        }
    }
}
