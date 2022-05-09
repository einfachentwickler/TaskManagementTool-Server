using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace TaskManagementTool.Host.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelStateFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.HttpContext.Response.WriteAsJsonAsync(context.ModelState);
            }

            await next();
        }
    }
}
