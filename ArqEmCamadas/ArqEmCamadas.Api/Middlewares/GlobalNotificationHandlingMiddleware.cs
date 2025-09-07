using ArqEmCamadas.Domain.Handlers.NotificationSettings;

namespace ArqEmCamadas.Api.Middlewares;

public sealed class GlobalNotificationHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            
            await httpContext.Response.WriteAsJsonAsync(
                new DomainNotification("Error", "Server side error"),
                CancellationToken.None);
        }
    } 
}