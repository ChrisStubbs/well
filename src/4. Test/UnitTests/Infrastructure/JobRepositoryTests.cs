

namespace PH.Well.UnitTests.Infrastructure
{
    using System.Collections.Generic;
    using System.Data;
    using Factories;
    using PH.Well.Common.Contracts;
    using Moq;
    using NUnit.Framework;
    
    using Repositories;
    using Repositories.Contracts;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Domain.ValueObjects;


    [TestFixture]
    public class JobRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IWellDapperProxy> dapperProxy;

        private JobRepository repository;
        private string UserName = "TestUser";

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);

            this.repository = new JobRepository(this.logger.Object, this.dapperProxy.Object);
            this.repository.CurrentUser = UserName;
        }

        public class TheGetByIdMethod : JobRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                const int id = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure("Job_GetById")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Id", id, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<Job>()).Returns(new List<Job>());

                var result = repository.GetById(id);

                dapperProxy.Verify(x => x.WithStoredProcedure("Job_GetById"), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("Id", id, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.Query<Job>(), Times.Once());
            }
        }

        public class TheGetCreditActionReasonsByIdMethod : JobRepositoryTests
        {
            [Test]
            public void ShouldReturnCreditActionReasons()
            {
                const int id = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobGetCreditActionReasons)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PDACreditReasonId", id, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<PodActionReasons>()).Returns(new List<PodActionReasons>());

                var result = repository.GetPodActionReasonsById(id);

                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobGetCreditActionReasons), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("PDACreditReasonId", id, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.Query<PodActionReasons>(), Times.Once());
            }
        }


        public class TheSaveJobMethod : JobRepositoryTests
        {
            [Test]
            public void ShouldSaveJob()
            {
                var job = JobFactory.New.Build();
                var user = UserFactory.New.Build();


                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobCreateOrUpdate)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Id", job.Id, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Sequence", job.Sequence, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Username", UserName, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobTypeCode", job.JobTypeCode, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PHAccount", job.PhAccount, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PickListRef", job.PickListRef, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("InvoiceNumber", string.IsNullOrWhiteSpace(job.InvoiceNumber) ? null : job.InvoiceNumber, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("CustomerRef", job.CustomerRef, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("OrderDate", job.OrderDate, DbType.DateTime, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("RoyaltyCode", job.RoyaltyCode, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("RoyaltyCodeDesc", job.RoyaltyCodeDesc, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("OrdOuters", job.OrdOuters, DbType.Int16, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("InvOuters", job.InvOuters, DbType.Int16, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ColOuters", job.ColOuters, DbType.Int16, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ColBoxes", job.ColBoxes, DbType.Int16, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ReCallPrd", job.ReCallPrd, DbType.Boolean, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("AllowSgCrd", job.AllowSgCrd, DbType.Boolean, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("AllowSOCrd", job.AllowSoCrd, DbType.Boolean, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("COD", job.Cod, DbType.Boolean, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("GrnNumber", job.GrnNumber, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("GrnRefusedReason", job.GrnRefusedReason, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("GrnRefusedDesc", job.GrnRefusedDesc, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("AllowReOrd", job.AllowReOrd, DbType.Boolean, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("SandwchOrd", job.SandwchOrd, DbType.Boolean, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ComdtyType", job.ComdtyType, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PerformanceStatusId", (int)job.PerformanceStatus, DbType.Int16, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ByPassReasonId  ", (int)job.ByPassReason, DbType.Int16, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("StopId", job.StopId, DbType.Int32, null)).Returns(dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new int[] { 1 });

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobGetById))
                    .Returns(dapperProxy.Object);

                dapperProxy.Setup(x => x.AddParameter("Id", job.Id, DbType.Int32, null))
                    .Returns(dapperProxy.Object);

                this.repository.JobCreateOrUpdate(job);

                Assert.That(user.Id, Is.EqualTo(1));

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobCreateOrUpdate), Times.Exactly(1));

                dapperProxy.Verify(x => x.AddParameter("Sequence", job.Sequence, DbType.Int32, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("Username", UserName, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("PHAccount", job.PhAccount, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("PickListRef", job.PickListRef, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("InvoiceNumber", string.IsNullOrWhiteSpace(job.InvoiceNumber) ? null : job.InvoiceNumber, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("CustomerRef", job.CustomerRef, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("OrderDate", job.OrderDate, DbType.DateTime, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("RoyaltyCode", job.RoyaltyCode, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("RoyaltyCodeDesc", job.RoyaltyCodeDesc, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("OrdOuters", job.OrdOuters, DbType.Int16, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("InvOuters", job.InvOuters, DbType.Int16, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ColOuters", job.ColOuters, DbType.Int16, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ColBoxes", job.ColBoxes, DbType.Int16, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ReCallPrd", job.ReCallPrd, DbType.Boolean, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("AllowSgCrd", job.AllowSgCrd, DbType.Boolean, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("AllowSOCrd", job.AllowSoCrd, DbType.Boolean, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("COD", job.Cod, DbType.Boolean, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("GrnNumber", job.GrnNumber, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("GrnRefusedReason", job.GrnRefusedReason, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("GrnRefusedDesc", job.GrnRefusedDesc, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("AllowReOrd", job.AllowReOrd, DbType.Boolean, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("SandwchOrd", job.SandwchOrd, DbType.Boolean, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ComdtyType", job.ComdtyType, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("PerformanceStatusId", (int) job.PerformanceStatus, DbType.Int16, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ByPassReasonId  ", (int)job.ByPassReason, DbType.Int16, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("StopId", job.StopId, DbType.Int32, null), Times.Exactly(1));


                this.dapperProxy.Verify(x => x.Query<int>(), Times.Exactly(1));
            }
        }


        public class TheGetByAccountPicklistAndStopId : JobRepositoryTests
        {
            [Test]
            public void ShouldGetByAccountPicklistAndStopId()
            {
                const int stopId = 1;
                const string accountId = "AC001";
                const string picklistId = "PKL001";

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobGetByAccountPicklistAndStopId)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("AccountId", accountId, DbType.String, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PicklistId", picklistId, DbType.String, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("StopId", stopId, DbType.Int32, null)).Returns(this.dapperProxy.Object);

                dapperProxy.Setup(x => x.Query<Job>()).Returns(new List<Job>());

                var result = this.repository.GetByAccountPicklistAndStopId(accountId, picklistId, stopId);

                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobGetByAccountPicklistAndStopId), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("AccountId", accountId, DbType.String, null), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("PicklistId", picklistId, DbType.String, null), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("StopId", stopId, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.Query<Job>(), Times.Once());
            }
        }

        public class TheDeleteJobByIdMethod : JobRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                var deleteType = WellDeleteType.SoftDelete;
                var isSoftDelete = deleteType == WellDeleteType.SoftDelete;

                const int id = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobDeleteById))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobId", id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("IsSoftDelete", isSoftDelete, DbType.Boolean, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Execute());

                this.repository.DeleteJobById(id, deleteType);

                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobDeleteById), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("JobId", id, DbType.Int32, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.AddParameter("IsSoftDelete", isSoftDelete, DbType.Boolean, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.Execute());

            }
        }

    }
}
