using ArqEmCamadas.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArqEmCamadas.Api.Filters;

public class NotificationFilter : ActionFilterAttribute
{
    private readonly INotificationHandler _notification;

    public NotificationFilter(INotificationHandler notification)
    {
        _notification = notification;
    }

    public override void OnActionExecuted(ActionExecutedContext actionContext)
    {
        if (_notification.HasNotification())
        {
            actionContext.Result = new BadRequestObjectResult(_notification.GetNotifications());
        }

        base.OnActionExecuted(actionContext);
    }
}