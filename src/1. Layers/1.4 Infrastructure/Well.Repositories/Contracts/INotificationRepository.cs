namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using PH.Well.Domain;

    public interface INotificationRepository : IRepository<Notification, int>
    {
        void SaveNotification(Notification notification, string connectionString);
        void SaveNotification(Notification notification);
        IEnumerable<Notification> GetNotifications(string connectionString);
        void ArchiveNotification(int id);
    }
}
