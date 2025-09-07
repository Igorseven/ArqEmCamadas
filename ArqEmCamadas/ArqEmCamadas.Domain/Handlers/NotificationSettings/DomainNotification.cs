namespace ArqEmCamadas.Domain.Handlers.NotificationSettings;

public sealed class DomainNotification(string key, string value)
{
    public string Key { get; set; } = key;
    public string Value { get; set; } = value;

    public static IEnumerable<DomainNotification> CreateNotifications(Dictionary<string, string> notifications)
    {
        return notifications.Select(notification => new DomainNotification(notification.Key, notification.Value));
    }
}