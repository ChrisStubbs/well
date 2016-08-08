namespace PH.Well.UnitTests.Infrastructure
{
    using System.Data;

    using Moq;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;

    [TestFixture]
    public class ExceptionEventRepositoryTests
    {
        private Mock<IDapperProxy> dapperProxy;

        private ExceptionEventRepository repository;

        [SetUp]
        public void Setup()
        {
            this.dapperProxy = new Mock<IDapperProxy>(MockBehavior.Strict);

            this.repository = new ExceptionEventRepository(this.dapperProxy.Object);
        }

        public class TheInsertCreditEventMethod : ExceptionEventRepositoryTests
        {
            [Test]
            public void ShouldInsertCreditEvent()
            {
                var creditEvent = new CreditEvent { BranchId = 2, InvoiceNumber = "012342.231" };

                var creditEventJson = JsonConvert.SerializeObject(creditEvent);

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.EventInsert))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Event", creditEventJson, DbType.String, 2500))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("ExceptionActionId", ExceptionAction.Credit, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute()); 

                this.repository.InsertCreditEvent(creditEvent);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.EventInsert), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Event", creditEventJson, DbType.String, 2500), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("ExceptionActionId", ExceptionAction.Credit, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.Execute(), Times.Once);
            }
        }
    }
}