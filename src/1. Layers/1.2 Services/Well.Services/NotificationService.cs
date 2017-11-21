namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Domain;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository notificationRepository;
        private readonly IDbMultiConfiguration multiDatabases;

        public NotificationService(INotificationRepository notificationRepository,
            IDbMultiConfiguration multiDatabases)
        {
            this.notificationRepository = notificationRepository;
            this.multiDatabases = multiDatabases;
        }

        public IList<Notification> GetNotificationsAllDatabases()
        {
            var notifications = new List<Notification>();
            foreach (var connectionString in multiDatabases.ConnectionStrings)
            {
                notifications.AddRange(GetNotifications(connectionString));
            }

            return notifications;
        }

        public IEnumerable<Notification> GetNotifications(string connectionString)
        {
            return notificationRepository.GetNotifications(connectionString);
        }

        public void SaveNotification(Notification notification, string connectionString)
        {
            this.notificationRepository.SaveNotification(notification, connectionString);
        }

        public void ArchiveNotification(int notificationId)
        {
            this.notificationRepository.ArchiveNotification(notificationId);
        }

    }
}