using ArqEmCamadas.Domain.Extensions;
using ArqEmCamadas.Domain.Handlers.LoggerHandler;
using ArqEmCamadas.Domain.Handlers.NotificationSettings;
using ArqEmCamadas.Domain.Interfaces;

namespace ArqEmCamadas.ApplicationService.Services;

public abstract class ServiceBase<T>(
    INotificationHandler notification,
    IValidate<T> validate,
    ILoggerHandler logger)
    where T : class
{
    protected readonly INotificationHandler Notification = notification;

    protected async Task<bool> EntityValidationAsync(T entity)
    {
        var validationResponse = await validate.ValidationAsync(entity);

        if (!validationResponse.Valid)
            Notification.CreateNotifications(DomainNotification.CreateNotifications(validationResponse.Errors));

        return validationResponse.Valid;
    }

    protected bool EntitiesValidation(List<T> entities)
    {
        foreach (var validationResponse in entities
                     .Select(validate.Validation)
                     .Where(validationResponse => !validationResponse.Valid))
        {
            Notification.CreateNotifications(DomainNotification.CreateNotifications(validationResponse.Errors));
        }

        return !Notification.HasNotification();
    }

    protected void GenerateLogger(
        string eventDescription,
        Guid userId,
        string? entityId = null)
    {
        var logger1 = new DomainLogger
        {
            ActionDate = DateTime.Now.GetDateAndTimeInBrasilia(),
            Description = eventDescription,
            UserId = userId,
            EntityId = entityId
        };

        logger.CreateLogger(logger1);
    }
}