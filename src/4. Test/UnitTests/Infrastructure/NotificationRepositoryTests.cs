namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Data;
    using PH.Well.Common.Contracts;
    using Domain;
    using Moq;
    using NUnit.Framework;
    using Repositories;
    using Repositories.Contracts;
    using System.Collections.Generic;
    using System.Linq;
    using Factories;
    using Well.Domain;

    [TestFixture]
    public class NotificationRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IWellDapperProxy> dapperProxy;

        private NotificationRepository repository;

        private Mock<IUserNameProvider> userNameProvider;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns("user");

            this.repository = new NotificationRepository(this.logger.Object, this.dapperProxy.Object, this.userNameProvider.Object);
        }

        public class TheGetNotificationsMethod : NotificationRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                dapperProxy.Setup(x => x.WithStoredProcedure("Notifications_Get")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<Notification>()).Returns(new List<Notification>());

                var result = repository.GetNotifications();

                dapperProxy.Verify(x => x.WithStoredProcedure("Notifications_Get"), Times.Once);
                dapperProxy.Verify(x => x.Query<Notification>(), Times.Once());
            }
        }

        public class TheArchiveNotificationMethod : NotificationRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                const int id = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure("Notification_Archive")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Id", id, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("UpdatedBy", It.IsAny<string>(), DbType.String, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Execute());

                repository.ArchiveNotification(id);

                dapperProxy.Verify(x => x.WithStoredProcedure("Notification_Archive"), Times.Once);
                dapperProxy.Verify(x => x.Execute(), Times.Once);
            }
        }

        public class TheSaveNotificationMethod : NotificationRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                var notification = NotificationFactory.New.Build();

                dapperProxy.Setup(x => x.WithStoredProcedure("Notification_Save")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobId", notification.JobId, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Type", notification.Type, DbType.Int16, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ErrorMessage", notification.ErrorMessage, DbType.String, 255)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Branch", notification.Branch, DbType.String, 3)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Account", notification.Account, DbType.String, 10)).Returns(this.dapperProxy.Object);

                dapperProxy.Setup(x => x.AddParameter("InvoiceNumber", notification.InvoiceNumber, DbType.String, 20)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("LineNumber", notification.LineNumber, DbType.String, 3)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("AdamErrorNumber", notification.AdamErrorNumber, DbType.String, 3)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("AdamCrossReference", notification.AdamCrossReference, DbType.String, 20)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("UserName", notification.UserName, DbType.String, 10)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("CreatedBy", It.IsAny<string>(), DbType.String, 50)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("UpdatedBy", It.IsAny<string>(), DbType.String, 50)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Execute());

                repository.SaveNotification(notification);

                dapperProxy.Verify(x => x.WithStoredProcedure("Notification_Save"), Times.Once);

                dapperProxy.Verify(x => x.AddParameter("JobId", notification.JobId, DbType.Int32, null), Times.Once());
                dapperProxy.Verify(x => x.AddParameter("ErrorMessage", It.IsAny<string>(), DbType.String, 255), Times.Once());
                dapperProxy.Verify(x => x.AddParameter("Branch", notification.Branch, DbType.String, 3), Times.Once());
                dapperProxy.Verify(x => x.AddParameter("Account", notification.Account, DbType.String, 10), Times.Once());

                dapperProxy.Verify(x => x.AddParameter("InvoiceNumber", notification.InvoiceNumber, DbType.String, 20), Times.Once());
                dapperProxy.Verify(x => x.AddParameter("LineNumber", notification.LineNumber, DbType.String, 3), Times.Once());
                dapperProxy.Verify(x => x.AddParameter("AdamErrorNumber", notification.AdamErrorNumber, DbType.String, 3), Times.Once());
                dapperProxy.Verify(x => x.AddParameter("AdamCrossReference", notification.AdamCrossReference, DbType.String, 20), Times.Once());
                dapperProxy.Verify(x => x.AddParameter("UserName", notification.UserName, DbType.String, 10), Times.Once());
                dapperProxy.Verify(x => x.AddParameter("CreatedBy", It.IsAny<string>(), DbType.String, 50), Times.Once());
                dapperProxy.Verify(x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once());
                dapperProxy.Verify(x => x.AddParameter("UpdatedBy", It.IsAny<string>(), DbType.String, 50), Times.Once());
                dapperProxy.Verify(x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once());

                dapperProxy.Verify(x => x.Execute(), Times.Once);

            }
        }
    }
}

