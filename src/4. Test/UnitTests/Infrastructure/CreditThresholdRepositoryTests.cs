namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;

    [TestFixture]
    public class CreditThresholdRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IDapperProxy> dapperProxy;

        private CreditThresholdRepository repository;

        private Mock<IUserNameProvider> userNameProvider;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IDapperProxy>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns("me");

            this.repository = new CreditThresholdRepository(this.logger.Object, this.dapperProxy.Object, this.userNameProvider.Object);
        }

        public class TheSaveMethod : CreditThresholdRepositoryTests
        {
            [Test]
            public void ShouldSaveTheCreditThreshold()
            {
                //////this.repository.CurrentUser = "me";

                var creditThreshold = new CreditThreshold
                {
                    ThresholdLevelId = 10,
                    Value = 100,
                    CreatedBy = "me",
                    UpdatedBy = "me"
                };

                var branch = new Branch { Id = 22 };

                creditThreshold.Branches.Add(branch);

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.CreditThresholdSave))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("ThresholdLevelId", creditThreshold.ThresholdLevelId, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("Value", creditThreshold.Value, DbType.Decimal, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("CreatedBy", creditThreshold.CreatedBy, DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("UpdatedBy", creditThreshold.UpdatedBy, DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new[] { 1 });

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.CreditThresholdToBranchSave))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("BranchId", branch.Id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("CreditThresholdId", 1, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute());

                this.repository.Save(creditThreshold);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.CreditThresholdSave), Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("ThresholdLevelId", creditThreshold.ThresholdLevelId, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("Value", creditThreshold.Value, DbType.Decimal, null), Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("CreatedBy", creditThreshold.CreatedBy, DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("UpdatedBy", creditThreshold.UpdatedBy, DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Once);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.CreditThresholdToBranchSave), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("BranchId", branch.Id, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("CreditThresholdId", 1, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.Execute(), Times.Once);
            }

            public void ShouldSaveTheCreditThresholdAndDeleteItselfIfTransient()
            {
                //////this.repository.CurrentUser = "me";

                var creditThreshold = new CreditThreshold
                {
                    Id = 101,
                    ThresholdLevelId = 10,
                    Value = 100,
                    CreatedBy = "me",
                    UpdatedBy = "me"
                };

                var branch = new Branch { Id = 22 };

                creditThreshold.Branches.Add(branch);

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.CreditThresholdDelete))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Id", 101, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.CreditThresholdSave))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("ThresholdLevelId", creditThreshold.ThresholdLevelId, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("Threshold", creditThreshold.Value, DbType.Decimal, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("CreatedBy", creditThreshold.CreatedBy, DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("UpdatedBy", creditThreshold.UpdatedBy, DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new[] { 1 });

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.CreditThresholdToBranchSave))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("BranchId", branch.Id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("CreditThresholdId", 1, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute());

                this.repository.Save(creditThreshold);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.CreditThresholdSave), Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("ThresholdLevelId", creditThreshold.ThresholdLevelId, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("Threshold", creditThreshold.Value, DbType.Decimal, null), Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("CreatedBy", creditThreshold.CreatedBy, DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("UpdatedBy", creditThreshold.UpdatedBy, DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Once);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.CreditThresholdToBranchSave), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("BranchId", branch.Id, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("CreditThresholdId", 1, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.Execute(), Times.Exactly(2));

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.CreditThresholdDelete), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Id", 101, DbType.Int32, null), Times.Once);
            }
        }

        public class TheGetByBranchMethod : CreditThresholdRepositoryTests
        {
            [Test]
            public void ShouldGetBranchSpecificThresholds()
            {
                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.CreditThresholdByBranch))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("branchId", 4, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<CreditThreshold>()).Returns(new List<CreditThreshold>());

                this.repository.GetByBranch(4);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.CreditThresholdByBranch), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("branchId", 4, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.Query<CreditThreshold>(), Times.Once);
            }
        }

        public class ThePendingCreditInsertMethod : CreditThresholdRepositoryTests
        {
            [Test]
            public void ShouldAddPendingCredit()
            {
                var user = new User { Id = 67 };

                var jobId = 45;

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.PendingCreditInsert))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("jobId", jobId, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("originator", "me", DbType.String, null))
                .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute());

                this.repository.PendingCreditInsert(jobId);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.PendingCreditInsert), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("jobId", jobId, DbType.Int32, null), Times.Once);

                //////this.dapperProxy.Verify(x => x.AddParameter("originator", "foo", DbType.String, null), Times.Once);

                this.dapperProxy.Verify(x => x.Execute(), Times.Once);
            }
        }

    }
}