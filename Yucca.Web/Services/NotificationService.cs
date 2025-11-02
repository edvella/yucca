using static Yucca.Web.Components.Shared.Notification;

namespace Yucca.Web.Services;

public class NotificationService
{
    public event Action<string, string, NotificationType>? OnNotificationShow;

    public void ShowNotification(string title, string message, NotificationType type = NotificationType.Info)
    {
        OnNotificationShow?.Invoke(title, message, type);
    }

    public void ShowSuccess(string message)
    {
        ShowNotification("Success", message, NotificationType.Success);
    }

    public void ShowError(string message)
    {
        ShowNotification("Error", message, NotificationType.Error);
    }

    public void ShowWarning(string message)
    {
        ShowNotification("Warning", message, NotificationType.Warning);
    }

    public void ShowInfo(string message)
    {
        ShowNotification("Information", message, NotificationType.Info);
    }
}
