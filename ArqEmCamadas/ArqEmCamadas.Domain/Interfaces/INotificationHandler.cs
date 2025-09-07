using ArqEmCamadas.Domain.Handlers.NotificationSettings;

namespace ArqEmCamadas.Domain.Interfaces;

public interface INotificationHandler
{
    List<DomainNotification> GetNotifications();
    bool HasNotification();
    bool CreateNotifications(IEnumerable<DomainNotification> notifications);
    void CreateNotification(DomainNotification notification);
    bool CreateNotification(string key, string value);
    void DeleteNotification(DomainNotification notification);
}