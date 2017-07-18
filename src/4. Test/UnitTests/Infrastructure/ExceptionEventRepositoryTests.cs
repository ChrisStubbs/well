namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Moq;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;

    [TestFixture]
    public class ExceptionEventRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IDapperProxy> dapperProxy;

        private ExceptionEventRepository repository;

        private Mock<IUserNameProvider> userNameProvider;

        [SetUp]
        public void Setup()
        {
            this.dapperProxy = new Mock<IDapperProxy>(MockBehavior.Strict);
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns("user");

            this.repository = new ExceptionEventRepository(this.logger.Object, this.dapperProxy.Object, this.userNameProvider.Object);
        }

        public class TheInsertCreditEventMethod : ExceptionEventRepositoryTests
        {
            [Test]
            public void ShouldInsertCreditEvent()
            {
                // todo
                /*this.repository.CurrentUser = "foo";
                var creditEvent = new CreditEvent { BranchId = 2, InvoiceNumber = "012342.231" };

                var creditEventJson = JsonConvert.SerializeObject(creditEvent);

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.EventInsert))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Event", creditEventJson, DbType.String, 2500))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("ExceptionActionId", EventAction.Credit, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("DateCanBeProcessed", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("CreatedBy", "foo", DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("UpdatedBy", "foo", DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute()); 

                this.repository.InsertCreditEvent(creditEvent);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.EventInsert), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Event", creditEventJson, DbType.String, 2500), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("ExceptionActionId", EventAction.Credit, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("DateCanBeProcessed", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.Execute(), Times.Once);*/
            }
        }

        public class TheMarkEventAsProcessedMethod : ExceptionEventRepositoryTests
        {
            public void ShouldSetTheProcessedFlagToTrue()
            {
                var eventId = 100;
                var username = "foo";

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.EventSetProcessed))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("UpdatedBy", username, DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.repository.MarkEventAsProcessed(eventId);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.EventSetProcessed), Times.Once);
            }
        }

        public class TheGetAllUnprocessedMethod : ExceptionEventRepositoryTests
        {
            [Test]
            public void ShouldReturnAllEventStillToBeProcessed()
            {
                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.EventGetUnprocessed))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<ExceptionEvent>()).Returns(new List<ExceptionEvent>());

                this.repository.GetAllUnprocessed();

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.EventGetUnprocessed), Times.Once);

                this.dapperProxy.Verify(x => x.Query<ExceptionEvent>(), Times.Once);
            }
        }

        public class TheInsertGrnMethod : ExceptionEventRepositoryTests
        {
            [Test]
            public void ShouldInsertGrnEvent()
            {
                var grnEvent = new GrnEvent {BranchId = 2, Id = 1};
                var grnEventJson = JsonConvert.SerializeObject(grnEvent);

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.EventInsert))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Event", It.IsAny<string>(), DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("ExceptionActionId", EventAction.Grn, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("DateCanBeProcessed", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("CreatedBy", "user", DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("UpdatedBy", "user", DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute());

                this.repository.InsertGrnEvent(grnEvent, DateTime.Now);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.EventInsert), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Event", It.IsAny<string>(), DbType.String, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("ExceptionActionId", EventAction.Grn, DbType.Int32, null),
                    Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("DateCanBeProcessed", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("CreatedBy", "user", DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("UpdatedBy", "user", DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.Execute(), Times.Once);


            }


            public class TheInsertPodMethod : ExceptionEventRepositoryTests
            {
                //[Test]
                //public void ShouldInsertPodEvent()
                //{
                //    var lines = new Dictionary<int, string>();

                //    lines.Add(1, "Thing 1");
                //    lines.Add(2, "Thing 2");

                //    var podTransaction = new PodTransaction {BranchId = 2, HeaderSql = "a sql string", LineSql = lines};
                //    var podTransactionJson = JsonConvert.SerializeObject(podTransaction);

                //    this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.EventInsert))
                //        .Returns(this.dapperProxy.Object);

                //    this.dapperProxy.Setup(x => x.AddParameter("Event", podTransactionJson, DbType.String, 2500))
                //        .Returns(this.dapperProxy.Object);

                //    this.dapperProxy.Setup(x => x.AddParameter("ExceptionActionId", EventAction.Pod, DbType.Int32, null))
                //        .Returns(this.dapperProxy.Object);

                //    this.dapperProxy.Setup(
                //        x => x.AddParameter("DateCanBeProcessed", It.IsAny<DateTime>(), DbType.DateTime, null))
                //        .Returns(this.dapperProxy.Object);

                //    this.dapperProxy.Setup(x => x.AddParameter("CreatedBy", "user", DbType.String, 50))
                //        .Returns(this.dapperProxy.Object);

                //    this.dapperProxy.Setup(x => x.AddParameter("UpdatedBy", "user", DbType.String, 50))
                //        .Returns(this.dapperProxy.Object);

                //    this.dapperProxy.Setup(
                //        x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null))
                //        .Returns(this.dapperProxy.Object);

                //    this.dapperProxy.Setup(
                //        x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null))
                //        .Returns(this.dapperProxy.Object);

                //    this.dapperProxy.Setup(x => x.Execute());

                //    this.repository.InsertPodEvent(podTransaction);

                //    this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.EventInsert), Times.Once);

                //    this.dapperProxy.Verify(x => x.AddParameter("Event", podTransactionJson, DbType.String, 2500),
                //        Times.Once);

                //    this.dapperProxy.Verify(
                //        x => x.AddParameter("ExceptionActionId", EventAction.Pod, DbType.Int32, null), Times.Once);

                //    this.dapperProxy.Verify(
                //        x => x.AddParameter("DateCanBeProcessed", It.IsAny<DateTime>(), DbType.DateTime, null),
                //        Times.Once);

                //    this.dapperProxy.Verify(x => x.AddParameter("CreatedBy", "user", DbType.String, 50), Times.Once);

                //    this.dapperProxy.Verify(x => x.AddParameter("UpdatedBy", "user", DbType.String, 50), Times.Once);

                //    this.dapperProxy.Verify(
                //        x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                //    this.dapperProxy.Verify(
                //        x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                //    this.dapperProxy.Verify(x => x.Execute(), Times.Once);
                //}
            }
        }

        public class TheInsertAmendmentTransactionMethod : ExceptionEventRepositoryTests
        {
            [Test]
            public void ShouldInsertAmendmentTransaction()
            {
                var lines = new Dictionary<int, string> {{1, "blah"}};
                var amendmentTransaction = new AmendmentTransaction { BranchId = 2, HeaderSql = "Blib blib blib", LineSql = lines};
                var amendmentJson = JsonConvert.SerializeObject(amendmentTransaction);

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.EventInsert))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Event", It.IsAny<string>(), DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("ExceptionActionId", EventAction.Amendment, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("DateCanBeProcessed", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("CreatedBy", "user", DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("UpdatedBy", "user", DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute());

                this.repository.InsertAmendmentTransaction(amendmentTransaction);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.EventInsert), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Event", It.IsAny<string>(), DbType.String, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("ExceptionActionId", EventAction.Amendment, DbType.Int32, null),
                    Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("DateCanBeProcessed", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("CreatedBy", "user", DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("UpdatedBy", "user", DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.Execute(), Times.Once);

            }
        }
    }
}