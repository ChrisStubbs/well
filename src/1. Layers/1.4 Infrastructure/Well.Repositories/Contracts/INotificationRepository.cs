namespace PH.Well.Repositories.Contracts
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PH.Well.Domain;

    public interface INotificationRepository : IRepository<Notification, int>
    {
        void SaveNotification(Notification notification);
        IEnumerable<Notification> GetNotifications();
        void ArchiveNotification(int id);
    }
}
