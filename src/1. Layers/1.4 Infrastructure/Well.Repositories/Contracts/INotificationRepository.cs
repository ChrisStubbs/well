using System.Collections;
using System.Collections.Generic;
using PH.Well.Domain;

namespace PH.Well.Repositories.Contracts
{
    public interface INotificationRepository : IRepository<Notification, int>
    {
        void SaveNotification(Notification notification);
        IEnumerable<Notification> GetNotifications();
        void ArchiveNotification(int id);
    }
}
