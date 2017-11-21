namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface INotificationService
    {
        IEnumerable<Notification> GetNotifications(string connectionString);
        IList<Notification> GetNotificationsAllDatabases();
        void SaveNotification(Notification notification,string connectionString);
        void ArchiveNotification(int notificationId);
    }
}