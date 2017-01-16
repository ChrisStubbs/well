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

        [SetUp]
        public void Setup()
        {
            this.dapperProxy = new Mock<IDapperProxy>(MockBehavior.Strict);
            this.logger = new Mock<ILogger>(MockBehavior.Strict);

            this.repository = new ExceptionEventRepository(this.logger.Object, this.dapperProxy.Object);
        }

        public class TheInsertCreditEventMethod : ExceptionEventRepositoryTests
        {
            [Test]
            public void ShouldInsertCreditEvent()
            {
                this.repository.CurrentUser = "foo";
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

                this.dapperProxy.Verify(x => x.Execute(), Times.Once);
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
    }
}