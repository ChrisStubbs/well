namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;
    using PH.Well.UnitTests.Factories;

    [TestFixture]
    public class CleanPreferenceRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IDapperProxy> dapperProxy;

        private CleanPreferenceRepository repository;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IDapperProxy>(MockBehavior.Strict);

            this.repository = new CleanPreferenceRepository(this.logger.Object, this.dapperProxy.Object);
        }

        public class TheGetAllMethod : CleanPreferenceRepositoryTests
        {
            [Test]
            public void ShouldReturnAllCleanPreferences()
            {
                var cleanPreferences = new List<CleanPreference>();

                var branch = BranchFactory.New.Build();

                cleanPreferences.Add(CleanPreferenceFactory.New.WithBranch(branch).Build());

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.CleanPreferencesGetAll))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("cleanPreferenceId", cleanPreferences[0].Id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<CleanPreference>()).Returns(cleanPreferences);

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.CleanPreferencesBranchesGet))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<Branch>()).Returns(new List<Branch>());

                var cleans = this.repository.GetAll();

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.CleanPreferencesGetAll), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("cleanPreferenceId", cleanPreferences[0].Id, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.CleanPreferencesBranchesGet), Times.Once);

                this.dapperProxy.Verify(x => x.Query<Branch>(), Times.Once);

                Assert.That(cleans.Count(), Is.EqualTo(1));
            }
        }

        public class TheDeleteMethod : CleanPreferenceRepositoryTests
        {
            [Test]
            public void ShouldHardDeleteTheCleanPreference()
            {
                var id = 12;

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.CleanPreferenceDelete))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Id", id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute());

                this.repository.Delete(id);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.CleanPreferenceDelete), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Id", id, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.Execute(), Times.Once);
            }
        }

        public class TheSaveMethod : CleanPreferenceRepositoryTests
        {
            [Test]
            public void ShouldSaveTheCleanPreferenceAndItsBranchAssociations()
            {
                var branch1 = new BranchFactory().Build();
                var branch2 = new BranchFactory().Build();
                var cleanPreference = CleanPreferenceFactory.New.With(x => x.Id = 0).WithBranch(branch1).WithBranch(branch2).Build();

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.CleanPreferenceSave))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("Days", cleanPreference.Days, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("CreatedBy", this.repository.CurrentUser, DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("UpdatedBy", this.repository.CurrentUser, DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new[] { 1 });

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.CleanPreferenceToBranchSave))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("BranchId", branch1.Id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("CleanPreferenceId", 1, DbType.Int32, null)).Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute());

                this.repository.Save(cleanPreference);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.CleanPreferenceSave), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Days", cleanPreference.Days, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("CreatedBy", this.repository.CurrentUser, DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("UpdatedBy", this.repository.CurrentUser, DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Once);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.CleanPreferenceToBranchSave), Times.Exactly(2));

                this.dapperProxy.Verify(x => x.AddParameter("BranchId", branch1.Id, DbType.Int32, null), Times.Exactly(2));

                this.dapperProxy.Verify(x => x.AddParameter("CleanPreferenceId", 1, DbType.Int32, null), Times.Exactly(2));

                this.dapperProxy.Verify(x => x.Execute(), Times.Exactly(2));
            }
        }

        public class TheGetByBranchIdMethod : CleanPreferenceRepositoryTests
        {
            [Test]
            public void ShouldGetCleanPreferenceByBranchId()
            {
                var branchId = 3;

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.CleanPreferenceByBranchGet))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("branchId", branchId, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<CleanPreference>()).Returns(new List<CleanPreference>());

                this.repository.GetByBranchId(branchId);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.CleanPreferenceByBranchGet), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("branchId", branchId, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.Query<CleanPreference>(), Times.Once);
            }
        }
    }
}