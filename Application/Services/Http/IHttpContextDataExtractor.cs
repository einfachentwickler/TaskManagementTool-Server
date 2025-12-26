using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Http;

public interface IHttpContextDataExtractor
{
    public string GetUserNameIdentifier(HttpContext context);

    public Task<bool> IsAllowedActionAsync(HttpContext context, int todoId, CancellationToken cancellationToken);
}