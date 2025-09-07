using ArqEmCamadas.Api.Extensions;
using ArqEmCamadas.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArqEmCamadas.Api.Filters;

public sealed class LoggerFilter(
    INotificationHandler notification, 
    ILoggerHandler logger)
    : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.IsMethodGet()) return;
        
        if (!SuccessFlow(context)) return;
        
        if (logger.HasLogger())
            logger.SaveInDataBase().Wait();

        base.OnActionExecuted(context);
    }
    
    private bool SuccessFlow(ActionExecutedContext context) =>
        context.Exception is null && 
        context.ModelState.IsValid && 
        !notification.HasNotification();
}