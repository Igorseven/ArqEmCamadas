using ArqEmCamadas.Api.Extensions;
using ArqEmCamadas.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArqEmCamadas.Api.Filters;

public sealed class  UnitOfWorkFilter(
    IUnitOfWork unitOfWork, 
    INotificationHandler notificationHandler)
    : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.IsMethodGet()) return;
        
        OthersMethods(context);

        base.OnActionExecuted(context);
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.IsMethodGet()) return;

        unitOfWork.BeginTransaction();

        base.OnActionExecuting(context);
    }
    
    private void OthersMethods(ActionExecutedContext context)
    {
        try
        {
            if (SuccessFlow(context))
                unitOfWork.CommitTransaction();
            else
                unitOfWork.RollbackTransaction();
        }
        catch
        {
            try
            {
                unitOfWork.RollbackTransaction();
            }
            catch
            {
                // ignored
            }

            throw;
        }
    }
        
    private bool SuccessFlow(ActionExecutedContext context) =>
        context.Exception is null && 
        context.ModelState.IsValid && 
        !notificationHandler.HasNotification();
}