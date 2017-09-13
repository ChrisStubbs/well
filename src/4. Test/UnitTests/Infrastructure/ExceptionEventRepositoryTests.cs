namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Moq;


    using NUnit.Framework;

    using Well.Common.Contracts;
    using PH.Well.Domain;
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