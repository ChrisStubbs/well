namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;

    public class NotificationRepository : DapperRepository<Notification, int>, INotificationRepository
    {
        public NotificationRepository(ILogger logger, IDapperProxy dapperProxy)
            : base(logger, dapperProxy)
        {
        }

        public void SaveNotification(Notification notification)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.SaveNotification)
                    .AddParameter("JobId", notification.JobId, DbType.Int32)
                    .AddParameter("Type", notification.Type, DbType.Int16)
                    .AddParameter("Reason", notification.ErrorMessage, DbType.String, size: 255)
                    .AddParameter("CreatedBy", notification.Source, DbType.String, size: 50)
                    .AddParameter("DateCreated", DateTime.Now, DbType.DateTime)
                    .AddParameter("UpdatedBy", notification.Source, DbType.String, size: 50)
                    .AddParameter("DateUpdated", DateTime.Now, DbType.DateTime)
                    .Execute();

        }

        public IEnumerable<Notification> GetNotifications()
        {

            return dapperProxy.WithStoredProcedure(StoredProcedures.GetNotifications)
              .Query<Notification>();

        }

        public void ArchiveNotification(int id)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.ArchiveNotification)
                    .AddParameter("Id", id, DbType.Int32)
                    .AddParameter("UpdatedBy", this.CurrentUser, DbType.String)
                    .AddParameter("DateUpdated", DateTime.Now, DbType.DateTime)
                    .Execute();
        }
    }
}
